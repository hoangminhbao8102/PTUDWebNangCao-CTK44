﻿namespace TatBlog.WebApp.Extensions
{
    public static class RouteExtensions
    {
        // Định nghĩa route template, route constraint cho các endpoints kết hợp với các action tron các controller.
        public static IEndpointRouteBuilder UseBlogRoutes(this IEndpointRouteBuilder endpoint) 
        {
            endpoint.MapControllerRoute(
                name: "posts-by-author",
                pattern: "blog/author/{slug}",
                defaults: new { controller = "Blog", action = "Author" });

            endpoint.MapControllerRoute(
                name: "posts-by-category",
                pattern: "blog/category/{slug}",
                defaults: new { controller = "Blog", action = "Category" });

            endpoint.MapControllerRoute(
                name: "posts-by-category",
                pattern: "blog/category/{slug}",
                defaults: new { controller = "Blog", action = "Category" });

            endpoint.MapControllerRoute(
                name: "posts-by-tag",
                pattern: "blog/tag/{slug}",
                defaults: new { controller = "Blog", action = "Tag" });

            endpoint.MapControllerRoute(
                name: "single-posts",
                pattern: "blog/post/{year:int}/{month:int}/{day:int}/{slug}",
                defaults: new { controller = "Blog", action = "Post" });

            endpoint.MapControllerRoute(
                name: "admin-area",
                pattern: "admin/{controller=Dashboard}/{action=Index}/{id?}",
                defaults: new { area = "Admin" });

            endpoint.MapControllerRoute(
                name: "default",
                pattern: "{controller=Blog}/{action=Index}/{id?}");

            return endpoint;
        }
    }
}
