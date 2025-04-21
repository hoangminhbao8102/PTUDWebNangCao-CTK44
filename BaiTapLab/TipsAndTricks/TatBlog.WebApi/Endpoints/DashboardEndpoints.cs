using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
    public static class DashboardEndpoints
    {
        public static WebApplication MapDashboardEndpoints(this WebApplication app)
        {
            var routeGroup = app.MapGroup("/api/dashboard");

            routeGroup.MapGet("/", GetDashboardStats)
                .WithName("GetDashboardStats")
                .Produces<ApiResponse<DashboardStats>>();

            return app;
        }

        private static async Task<IResult> GetDashboardStats(
            [FromServices] IDashboardRepository dashboardRepo)
        {
            var stats = new DashboardStats
            {
                TotalPosts = await dashboardRepo.CountTotalPostsAsync(),
                UnpublishedPosts = await dashboardRepo.CountUnpublishedPostsAsync(),
                TotalCategories = await dashboardRepo.CountCategoriesAsync(),
                TotalAuthors = await dashboardRepo.CountAuthorsAsync(),
                PendingComments = await dashboardRepo.CountPendingCommentsAsync(),
                TotalSubscribers = await dashboardRepo.CountSubscribersAsync(),
                NewSubscribersToday = await dashboardRepo.CountNewSubscribersTodayAsync()
            };

            return Results.Ok(ApiResponse.Success(stats));
        }
    }
}
