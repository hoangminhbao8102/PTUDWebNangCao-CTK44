using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using TatBlog.Data.Contexts;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.Services.Timing;
using TatBlog.WebApi.Models;
using TatBlog.WebApi.Validations;

namespace TatBlog.WebApi.Extensions
{
    public static class WebApplicationExtensions
    {
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddMemoryCache();

            builder.Services.AddDbContext<BlogDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes("tatblog-secret-key")) // 🔐 Replace this with real key
                    };
                });

            builder.Services.AddAuthorization();

            // Đăng ký các DI khác
            builder.Services
                .AddScoped<ITimeProvider, LocalTimeProvider>()
                .AddScoped<IMediaManager, LocalFileSystemMediaManager>()
                .AddScoped<IBlogRepository, BlogRepository>()
                .AddScoped<IAuthorRepository, AuthorRepository>()
                .AddScoped<ITagRepository, TagRepository>()
                .AddScoped<ICommentRepository, CommentRepository>()
                .AddScoped<ISubscriberRepository, SubscriberRepository>()
                .AddScoped<IFeedbackRepository, FeedbackRepository>()
                .AddScoped<IDashboardRepository, DashboardRepository>()
                .AddScoped<IValidator<FeedbackEditModel>, FeedbackValidator>();

            return builder;
        }

        public static WebApplicationBuilder ConfigureCors(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(option=>
            {
                option.AddPolicy("TatBlogApp", policyBuilder => 
                    policyBuilder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            return builder;
        }

        // Cấu hình việc sử dụng NLog
        public static WebApplicationBuilder ConfigureNLog(this WebApplicationBuilder builder)
        {
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Host.UseNLog();

            return builder;
        }

        public static WebApplicationBuilder ConfigureSwaggerOpenApi(this WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            return builder;
        }

        public static WebApplication SetupRequestPipeline(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("TatBlogApp");

            // 👇 THÊM dòng này để xử lý [Authorize]
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
