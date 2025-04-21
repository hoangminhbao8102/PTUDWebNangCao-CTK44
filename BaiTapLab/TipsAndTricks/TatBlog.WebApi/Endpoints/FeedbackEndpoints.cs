using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
    public static class FeedbackEndpoints
    {
        public static WebApplication MapFeedbackEndpoints(this WebApplication app)
        {
            var routeGroup = app.MapGroup("/api/feedback");

            routeGroup.MapPost("/", SendFeedback)
                .WithName("SendFeedback")
                .AddEndpointFilter<ValidatorFilter<FeedbackEditModel>>()
                .Produces<ApiResponse<string>>()
                .Produces(400);

            return app;
        }

        private static async Task<IResult> SendFeedback(
            [FromBody] FeedbackEditModel model,
            [FromServices] IFeedbackRepository feedbackRepo,
            [FromServices] IMapper mapper)
        {
            var feedback = mapper.Map<Feedback>(model);
            feedback.SentDate = DateTime.UtcNow;

            var result = await feedbackRepo.AddFeedbackAsync(feedback);

            return result
                ? Results.Ok(ApiResponse.Success("Gửi góp ý thành công."))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.InternalServerError, "Không thể gửi góp ý."));
        }
    }
}
