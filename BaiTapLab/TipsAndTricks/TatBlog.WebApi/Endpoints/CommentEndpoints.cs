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
    public static class CommentEndpoints
    {
        public static WebApplication MapCommentEndpoints(this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/comments");

            routeGroupBuilder.MapGet("/", GetComments)
                .WithName("GetComments")
                .Produces<ApiResponse<PaginationResult<CommentItem>>>();

            routeGroupBuilder.MapGet("/{id:int}", GetCommentById)
                .WithName("GetCommentById")
                .Produces<ApiResponse<CommentItem>>()
                .Produces(404);

            routeGroupBuilder.MapPost("/", AddComment)
                .WithName("AddNewComment")
                .AddEndpointFilter<ValidatorFilter<CommentEditModel>>()
                .Produces<ApiResponse<CommentItem>>()
                .Produces(400);

            routeGroupBuilder.MapPut("/{id:int}", UpdateComment)
                .WithName("UpdateComment")
                .AddEndpointFilter<ValidatorFilter<CommentEditModel>>()
                .Produces<ApiResponse<string>>()
                .Produces(204)
                .Produces(404);

            routeGroupBuilder.MapDelete("/{id:int}", DeleteComment)
                .WithName("DeleteComment")
                .Produces<ApiResponse<string>>()
                .Produces(204)
                .Produces(404);

            routeGroupBuilder.MapPut("/{id:int}/approve", ApproveComment)
                .WithName("ApproveComment")
                .Produces<ApiResponse<string>>()
                .Produces(204)
                .Produces(404);

            return app;
        }

        private static async Task<IResult> GetComments(
            [AsParameters] CommentFilterModel model,
            [FromServices] ICommentRepository commentRepo)
        {
            var commentList = await commentRepo.GetPagedCommentsAsync(model);
            var result = new PaginationResult<CommentItem>(commentList);

            return Results.Ok(ApiResponse.Success(result));
        }

        private static async Task<IResult> GetCommentById(
            int id, [FromServices] ICommentRepository commentRepo, [FromServices] IMapper mapper)
        {
            var comment = await commentRepo.GetCommentByIdAsync(id);
            return comment == null
                ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Không tìm thấy bình luận"))
                : Results.Ok(ApiResponse.Success(mapper.Map<CommentItem>(comment)));
        }

        private static async Task<IResult> AddComment(
            [FromBody] CommentEditModel model,
            [FromServices] ICommentRepository commentRepo,
            [FromServices] IMapper mapper)
        {
            var comment = mapper.Map<Comment>(model);
            await commentRepo.AddOrUpdateAsync(comment);
            return Results.Ok(ApiResponse.Success(mapper.Map<CommentItem>(comment), HttpStatusCode.Created));
        }

        private static async Task<IResult> UpdateComment(
            int id, [FromBody] CommentEditModel model,
            [FromServices] ICommentRepository commentRepo,
            [FromServices] IMapper mapper)
        {
            var existingComment = await commentRepo.GetCommentByIdAsync(id);
            if (existingComment == null)
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Không tìm thấy bình luận"));

            var comment = mapper.Map(model, existingComment);
            comment.Id = id;

            await commentRepo.AddOrUpdateAsync(comment);
            return Results.Ok(ApiResponse.Success("Bình luận đã cập nhật", HttpStatusCode.NoContent));
        }

        private static async Task<IResult> DeleteComment(
            int id, [FromServices] ICommentRepository commentRepo)
        {
            return await commentRepo.DeleteCommentAsync(id)
                ? Results.Ok(ApiResponse.Success("Bình luận đã xóa", HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Không tìm thấy bình luận"));
        }

        private static async Task<IResult> ApproveComment(
            int id, [FromServices] ICommentRepository commentRepo)
        {
            var success = await commentRepo.ApproveCommentAsync(id);
            return success
                ? Results.Ok(ApiResponse.Success("Bình luận đã được phê duyệt", HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Không tìm thấy bình luận để phê duyệt"));
        }
    }
}
