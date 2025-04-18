using TatBlog.WebApi.Endpoints;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Mapsters;
using TatBlog.WebApi.Validations;
using NLog;
using NLog.Web;

var logger = LogManager
    .Setup()
    .LoadConfigurationFromAppSettings()
    .GetCurrentClassLogger();

try
{
    logger.Info("Khởi động ứng dụng...");

    var builder = WebApplication.CreateBuilder(args);

    // Gọi ConfigureNLog để đăng ký NLog vào hệ thống logging
    builder.ConfigureNLog();  // Vẫn giữ nếu bạn đang dùng extension riêng

    builder
        .ConfigureCors()
        .ConfigureServices()
        .ConfigureSwaggerOpenApi()
        .ConfigureMapster()
        .ConfigureFluentValidation();

    var app = builder.Build();

    app.SetupRequestPipeline();

    app.MapAuthorEndpoints();

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Ứng dụng dừng do lỗi nghiêm trọng.");
    throw;
}
finally
{
    LogManager.Shutdown();
}
