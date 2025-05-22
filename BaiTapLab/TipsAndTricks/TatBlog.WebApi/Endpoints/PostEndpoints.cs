using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApi.Models;
using TatBlog.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace TatBlog.WebApi.Endpoints
{
    public static class PostEndpoints
    {
        public static WebApplication MapPostEndpoints(this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/posts");

            routeGroupBuilder.MapGet("/get-posts-filter", GetFilteredPosts)
                .WithName("GetFilteredPostByQuery")
                .Produces<ApiResponse<PostDto>>();

            routeGroupBuilder.MapGet("/get-filter", GetFilter)
                .WithName("GetFilter")
                .Produces<ApiResponse<PostDto>>();

            routeGroupBuilder.MapGet("/", GetFilteredPosts)
                .WithName("GetFilteredPostBasic")
                .Produces<ApiResponse<PaginationResult<PostDto>>>();

            routeGroupBuilder.MapGet("/featured/{limit:int}", GetFeaturedPosts)
                .WithName("GetFeaturedPosts")
                .Produces<ApiResponse<IList<PostDto>>>();

            routeGroupBuilder.MapGet("/random/{limit:int}", GetRandomPosts)
                .WithName("GetRandomPosts")
                .Produces<ApiResponse<IList<PostDto>>>();

            routeGroupBuilder.MapGet("/archives/{limit:int}", GetArchives)
                .WithName("GetPostArchives")
                .Produces<ApiResponse<IList<MonthlyPostCount>>>();

            routeGroupBuilder.MapGet("/{id:int}", GetPostById)
                .WithName("GetPostById")
                .Produces<ApiResponse<PostDto>>();

            routeGroupBuilder.MapGet("/byslug/{slug:regex(^[a-z0-9-]+$)}", GetPostBySlug)
                .WithName("GetPostBySlug")
                .Produces<ApiResponse<PostDto>>();

            routeGroupBuilder.MapPost("/", AddPost)
                .WithName("AddNewPost")
                .Accepts<PostEditModel>("multipart/form-data")
                .Produces(401)
                .Produces<ApiResponse<PostItem>>();

            routeGroupBuilder.MapPost("/{id:int}/picture", UploadPostImage)
               .Accepts<IFormFile>("multipart/form-data")
               .WithName("UploadPostImage")
               .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapPut("/{id:int}", UpdatePost)
                .WithName("UpdatePost")
                .Accepts<PostEditModel>("multipart/form-data")
                .Produces<ApiResponse>();

            routeGroupBuilder.MapDelete("/{id:int}", DeletePost)
                .WithName("DeletePost")
                .Produces<ApiResponse>();

            routeGroupBuilder.MapGet("/{id:int}/comments", GetPostComments)
                .WithName("GetPostComments")
                .Produces<ApiResponse<IList<CommentDto>>>();

            return app;
        }

        private static async Task<IResult> GetFilter(
            IAuthorRepository authorRepository,
            IBlogRepository blogRepository)
        {
            var model = new PostFilterModel()
            {
                AuthorList = (await authorRepository.GetAuthorsAsync())
                .Select(a => new SelectListItem()
                {
                    Text = a.FullName,
                    Value = a.Id.ToString()
                }),
                CategoryList = (await blogRepository.GetCategoriesAsync())
                .Select(a => new SelectListItem()
                {
                    Text = a.Name,
                    Value = a.Id.ToString()
                })
            };

            return Results.Ok(ApiResponse.Success(model));
        }

        private static async Task<IResult> GetFilteredPosts(
            [AsParameters] PostFilterModel model,
            [AsParameters] PagingModel pagingModel,
            IBlogRepository blogRepository)
        {
            var postQuery = new PostQuery()
            {
                Keyword = model.Keyword,
                CategoryId = model.CategoryId,
                AuthorId = model.AuthorId,
                Year = model.Year,
                Month = model.Month,
            };

            var postsList = await blogRepository.GetPagedPostsAsync(postQuery, pagingModel, posts => posts.ProjectToType<PostDto>());

            var paginationResult = new PaginationResult<PostDto>(postsList);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> GetFeaturedPosts(int limit, IBlogRepository blogRepo)
        {
            var posts = await blogRepo.GetPopularArticlesAsync(limit);
            var result = posts.Adapt<List<PostDto>>(); // ✅ Dùng Adapt cho List
            return Results.Ok(ApiResponse.Success(result));
        }

        private static async Task<IResult> GetRandomPosts(int limit, IBlogRepository blogRepo)
        {
            var posts = await blogRepo.GetRandomPostsAsync(limit);
            var result = posts.Adapt<List<PostDto>>(); // ✅ Dùng Adapt cho List
            return Results.Ok(ApiResponse.Success(result));
        }

        private static async Task<IResult> GetArchives(int limit, IBlogRepository blogRepo)
        {
            var archives = await blogRepo.CountPostsInRecentMonthsAsync(limit);
            return Results.Ok(ApiResponse.Success(archives));
        }

        private static async Task<IResult> GetPostById(int id, IBlogRepository blogRepo, IMapper mapper)
        {
            var post = await blogRepo.GetPostByIdAsync(id);
            return post == null
                ? Results.NotFound(ApiResponse.Fail(HttpStatusCode.NotFound, $"Post id = {id} not found"))
                : Results.Ok(ApiResponse.Success(mapper.Map<PostDto>(post)));
        }

        private static async Task<IResult> GetPostBySlug(string slug, IBlogRepository blogRepo, IMapper mapper)
        {
            var post = await blogRepo.GetPostBySlugAsync(slug);
            return post == null
                ? Results.NotFound(ApiResponse.Fail(HttpStatusCode.NotFound, $"Post slug = '{slug}' not found"))
                : Results.Ok(ApiResponse.Success(mapper.Map<PostDto>(post)));
        }

        private static async Task<IResult> AddPost(
            HttpContext context,
            IBlogRepository blogRepository,
            IMapper mapper,
            IMediaManager mediaManager)
        {
            var model = await PostEditModel.BindAsync(context);
            var slug = model.Title.GenerateSlug();
            if (await blogRepository.IsPostSlugExistedAsync(model.Id, slug))
            {
                return Results.Ok(ApiResponse.Fail(
                    HttpStatusCode.Conflict, $"Slug '{slug}' đã được sử dụng cho bài viết khác"));
            }

            var post = model.Id > 0 ? await blogRepository.GetPostByIdAsync(model.Id) : null;

            if (post == null)
            {
                post = new Post()
                {
                    PostedDate = DateTime.Now
                };
            }

            post.Title = model.Title;
            post.AuthorId = model.AuthorId;
            post.CategoryId = model.CategoryId;
            post.ShortDescription = model.ShortDescription;
            post.Description = model.Description;
            post.Meta = model.Meta;
            post.Published = model.Published;
            post.ModifiedDate = DateTime.Now;
            post.UrlSlug = model.Title.GenerateSlug();

            if (model.ImageFile?.Length > 0)
            {
                string hostname = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}/",
                    uploadedPath = await mediaManager.SaveFileAsync(model.ImageFile.OpenReadStream(),
                    model.ImageFile.FileName,
                    model.ImageFile.ContentType);

                if (!string.IsNullOrWhiteSpace(uploadedPath))
                {
                    post.ImageUrl = hostname + uploadedPath;
                }
            }

            await blogRepository.CreateOrUpdatePostAsync(post, model.GetSelectedTags());

            return Results.Ok(ApiResponse.Success(mapper.Map<PostItem>(post), HttpStatusCode.Created));
        }

        private static async Task<IResult> UpdatePost(
            int id,
            HttpContext context,
            [FromServices] IBlogRepository blogRepo,
            [FromServices] IMapper mapper,
            [FromServices] IMediaManager mediaManager)
        {
            var model = await PostEditModel.BindAsync(context);
            var slug = model.Title.GenerateSlug();

            if (await blogRepo.IsPostSlugExistedAsync(id, slug))
                return Results.Conflict(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{slug}' already exists"));

            var post = await blogRepo.GetPostByIdAsync(id);
            if (post == null)
                return Results.NotFound(ApiResponse.Fail(HttpStatusCode.NotFound, $"Post id = {id} not found"));

            mapper.Map(model, post);
            post.UrlSlug = slug;
            post.ModifiedDate = DateTime.Now;

            if (model.ImageFile != null)
            {
                string imageUrl = await mediaManager.SaveFileAsync(model.ImageFile.OpenReadStream(), model.ImageFile.FileName, model.ImageFile.ContentType);
                post.ImageUrl = $"{context.Request.Scheme}://{context.Request.Host}/{imageUrl}";
            }

            await blogRepo.CreateOrUpdatePostAsync(post, model.GetSelectedTags());
            return Results.Ok(ApiResponse.Success("Bài viết đã được cập nhật"));
        }

        private static async Task<IResult> UploadPostImage(
            HttpRequest request,
            int id,
            IMediaManager mediaManager)
        {
            var form = await request.ReadFormAsync();
            var file = form.Files["imageFile"];

            if (file == null || file.Length == 0)
            {
                return Results.BadRequest(ApiResponse.Fail(HttpStatusCode.BadRequest, "No file uploaded"));
            }

            var imagePath = await mediaManager.SaveFileAsync(file.OpenReadStream(), file.FileName, file.ContentType);

            return string.IsNullOrWhiteSpace(imagePath)
                ? Results.BadRequest(ApiResponse.Fail(HttpStatusCode.BadRequest, "Upload failed"))
                : Results.Ok(ApiResponse.Success(imagePath));
        }

        private static async Task<IResult> DeletePost(int id, IBlogRepository blogRepo)
        {
            return await blogRepo.DeletePostByIdAsync(id)
                ? Results.Ok(ApiResponse.Success("Bài viết đã được xóa"))
                : Results.NotFound(ApiResponse.Fail(HttpStatusCode.NotFound, $"Post id = {id} not found"));
        }

        private static async Task<IResult> GetPostComments(
            int id,
            [FromServices] ICommentRepository commentRepo)
        {
            var comments = await commentRepo.GetCommentsAsync(id);
            var commentDtos = comments.Adapt<List<CommentDto>>();
            return Results.Ok(ApiResponse.Success(commentDtos));
        }
    }
}
