using MapsterMapper;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
    public static class CategoryEndpoints
    {
        public static WebApplication MapCategoryEndpoints(this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/categories");

            // GET: /api/categories
            routeGroupBuilder.MapGet("/", GetCategories)
                .WithName("GetCategories")
                .Produces<ApiResponse<PaginationResult<CategoryItem>>>();

            // GET: /api/categories/{id}
            routeGroupBuilder.MapGet("/{id:int}", GetCategoryById)
                .WithName("GetCategoryById")
                .Produces<ApiResponse<CategoryItem>>();

            // GET: /api/categories/{slug}/posts
            routeGroupBuilder.MapGet("/{slug:regex(^[a-z0-9-]+$)}/posts", GetPostsByCategorySlug)
                .WithName("GetPostsByCategorySlug")
                .Produces<ApiResponse<PaginationResult<PostDto>>>();

            // POST: /api/categories
            routeGroupBuilder.MapPost("/", AddCategory)
                .WithName("AddCategory")
                .Produces<ApiResponse<CategoryItem>>();

            // PUT: /api/categories/{id}
            routeGroupBuilder.MapPut("/{id:int}", UpdateCategory)
                .WithName("UpdateCategory")
                .Produces<ApiResponse>();

            // DELETE: /api/categories/{id}
            routeGroupBuilder.MapDelete("/{id:int}", DeleteCategory)
                .WithName("DeleteCategory")
                .Produces<ApiResponse>();

            return app;
        }

        private static async Task<IResult> GetCategories(IBlogRepository blogRepository)
        {
            var categories = await blogRepository.GetCategoriesAsync();
            return Results.Ok(ApiResponse.Success(categories));
        }

        private static async Task<IResult> GetCategoryById(
            int id,
            IBlogRepository blogRepository)
        {
            var category = await blogRepository.GetCategoryByIdAsync(id);

            return category == null
                ? Results.NotFound(ApiResponse.Fail(HttpStatusCode.NotFound, "Không tìm thấy chuyên mục."))
                : Results.Ok(ApiResponse.Success(category));
        }

        private static async Task<IResult> GetPostsByCategorySlug(
            string slug,
            [AsParameters] PagingModel model,
            IBlogRepository blogRepository)
        {
            var pagingParams = new PagingModel
            {
                PageNumber = model.PageNumber,
                PageSize = model.PageSize,
                SortColumn = model.SortColumn,
                SortOrder = model.SortOrder
            };

            var posts = await blogRepository.GetPostsByCategorySlugAsync(slug, pagingParams);
            return Results.Ok(ApiResponse.Success(posts));
        }

        private static async Task<IResult> AddCategory(
            CategoryEditModel model,
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            // Kiểm tra slug đã tồn tại chưa
            if (await blogRepository.IsCategorySlugExistedAsync(0, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(
                    HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            // Ánh xạ DTO -> Entity
            var category = mapper.Map<Category>(model);

            // Lưu vào DB
            await blogRepository.CreateOrUpdateCategoryAsync(category);

            // Trả về dữ liệu đã thêm với status 201 Created
            return Results.Ok(ApiResponse.Success(
                mapper.Map<CategoryItem>(category), HttpStatusCode.Created));
        }

        private static async Task<IResult> UpdateCategory(
            int id,
            CategoryEditModel model,
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            // Kiểm tra slug đã tồn tại ở một chuyên mục khác
            if (await blogRepository.IsCategorySlugExistedAsync(id, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(
                    HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            // Ánh xạ DTO -> Entity và gán ID
            var category = mapper.Map<Category>(model);
            category.Id = id;

            // Gọi repository để thêm/cập nhật
            var result = await blogRepository.CreateOrUpdateCategoryAsync(category);

            // Nếu không tìm thấy (có thể do Id không hợp lệ)
            if (result == null)
            {
                return Results.Ok(ApiResponse.Fail(
                    HttpStatusCode.NotFound, "Không tìm thấy chuyên mục"));
            }

            // Trả về thành công với HTTP 204 NoContent
            return Results.Ok(ApiResponse.Success("Chuyên mục đã được cập nhật", HttpStatusCode.NoContent));
        }


        private static async Task<IResult> DeleteCategory(
            int id,
            IBlogRepository blogRepository)
        {
            var result = await blogRepository.DeleteCategoryByIdAsync(id);
            return result
                ? Results.Ok(ApiResponse.Success("Xóa chuyên mục thành công."))
                : Results.NotFound(ApiResponse.Fail(HttpStatusCode.NotFound, "Không tìm thấy chuyên mục."));
        }
    }
}
