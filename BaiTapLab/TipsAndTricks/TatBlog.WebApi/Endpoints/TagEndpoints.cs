using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
    public static class TagEndpoints
    {
        public static WebApplication MapTagEndpoints(this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/tags");

            routeGroupBuilder.MapGet("/", GetTags)
                .WithName("GetTags")
                .Produces<ApiResponse<PaginationResult<TagItem>>>();

            routeGroupBuilder.MapGet("/{id:int}", GetTagDetails)
                .WithName("GetTagById")
                .Produces<ApiResponse<TagItem>>()
                .Produces(404);

            routeGroupBuilder.MapPost("/", AddTag)
                .WithName("AddNewTag")
                .AddEndpointFilter<ValidatorFilter<TagEditModel>>()
                .Produces<ApiResponse<TagItem>>()
                .Produces(400)
                .Produces(409);

            routeGroupBuilder.MapPut("/{id:int}", UpdateTag)
                .WithName("UpdateTag")
                .AddEndpointFilter<ValidatorFilter<TagEditModel>>()
                .Produces<ApiResponse<string>>()
                .Produces(204)
                .Produces(400)
                .Produces(409);

            routeGroupBuilder.MapDelete("/{id:int}", DeleteTag)
                .WithName("DeleteTag")
                .Produces<ApiResponse<string>>()
                .Produces(204)
                .Produces(404);

            return app;
        }

        private static async Task<IResult> GetTags(
            [FromQuery] string name,
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize,
            [FromServices] ITagRepository tagRepository)
        {
            var filter = new TagFilterModel
            {
                Name = name,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var tagsList = await tagRepository.GetPagedTagsAsync(filter, name);

            var result = new PaginationResult<TagItem>(tagsList);
            return Results.Ok(ApiResponse.Success(result));
        }

        private static async Task<IResult> GetTagDetails(
            int id,
            [FromServices] ITagRepository tagRepository,
            [FromServices] IMapper mapper)
        {
            var tag = await tagRepository.GetCachedTagByIdAsync(id);

            return tag == null
                ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy thẻ có mã số {id}"))
                : Results.Ok(ApiResponse.Success(mapper.Map<TagItem>(tag)));
        }

        private static async Task<IResult> AddTag(
            TagEditModel model,
            [FromServices] ITagRepository tagRepository,
            [FromServices] IMapper mapper)
        {
            if (await tagRepository.IsTagSlugExistedAsync(0, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(
                    HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            var tag = mapper.Map<Tag>(model);
            await tagRepository.AddOrUpdateAsync(tag);

            return Results.Ok(ApiResponse.Success(mapper.Map<TagItem>(tag), HttpStatusCode.Created));
        }

        private static async Task<IResult> UpdateTag(
            int id, TagEditModel model,
            [FromServices] ITagRepository tagRepository,
            [FromServices] IMapper mapper)
        {
            if (await tagRepository.IsTagSlugExistedAsync(id, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(
                    HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            var tag = mapper.Map<Tag>(model);
            tag.Id = id;

            return await tagRepository.AddOrUpdateAsync(tag)
                ? Results.Ok(ApiResponse.Success("Thẻ đã được cập nhật", HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Không tìm thấy thẻ"));
        }

        private static async Task<IResult> DeleteTag(
            int id, [FromServices] ITagRepository tagRepository)
        {
            return await tagRepository.DeleteTagAsync(id)
                ? Results.Ok(ApiResponse.Success("Thẻ đã bị xóa", HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Không tìm thấy thẻ"));
        }
    }
}
