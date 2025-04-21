using Microsoft.AspNetCore.Mvc;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
    public static class SubscriberEndpoints
    {
        public static WebApplication MapSubscriberEndpoints(this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/subscribers");

            routeGroupBuilder.MapGet("/", GetSubscribers)
                .WithName("GetSubscribers")
                .Produces<ApiResponse<PaginationResult<Subscriber>>>(); // hoặc SubscriberDto nếu có

            routeGroupBuilder.MapPost("/subscribe", Subscribe)
                .WithName("SubscribeToBlog")
                .AddEndpointFilter<ValidatorFilter<SubscriberEditModel>>()
                .Produces<ApiResponse<string>>()
                .Produces(400)
                .Produces(409);

            routeGroupBuilder.MapPost("/unsubscribe", Unsubscribe)
                .WithName("UnsubscribeFromBlog")
                .AddEndpointFilter<ValidatorFilter<SubscriberEditModel>>()
                .Produces<ApiResponse<string>>()
                .Produces(400)
                .Produces(404);

            return app;
        }

        private static async Task<IResult> GetSubscribers(
            [AsParameters] PagingModel pagingModel,
            [FromServices] ISubscriberRepository subscriberRepo)
        {
            var subscribers = await subscriberRepo.GetPagedSubscribersAsync(pagingModel);
            var result = new PaginationResult<Subscriber>(subscribers);
            return Results.Ok(ApiResponse.Success(result));
        }

        private static async Task<IResult> Subscribe(
            [FromBody] SubscriberEditModel model,
            [FromServices] ISubscriberRepository subscriberRepo)
        {
            var existing = await subscriberRepo.GetSubscriberByEmailAsync(model.Email);
            if (existing != null)
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, "Email đã đăng ký theo dõi"));
            }

            await subscriberRepo.SubscribeAsync(model.Email);
            return Results.Ok(ApiResponse.Success("Đăng ký theo dõi thành công"));
        }

        private static async Task<IResult> Unsubscribe(
            [FromBody] SubscriberEditModel model,
            [FromServices] ISubscriberRepository subscriberRepo)
        {
            var existing = await subscriberRepo.GetSubscriberByEmailAsync(model.Email);
            if (existing == null)
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Email chưa đăng ký theo dõi"));
            }

            await subscriberRepo.UnsubscribeAsync(model.Email, model.Reason, model.Voluntary);
            return Results.Ok(ApiResponse.Success("Hủy đăng ký thành công"));
        }
    }
}
