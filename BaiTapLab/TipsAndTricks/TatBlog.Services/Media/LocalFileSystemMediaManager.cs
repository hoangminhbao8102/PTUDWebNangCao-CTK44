﻿using Microsoft.Extensions.Logging;

namespace TatBlog.Services.Media
{
    public class LocalFileSystemMediaManager : IMediaManager
    {
        private const string PicturesFolder = "uploads/pictures/{0}{1}";
        private readonly ILogger<LocalFileSystemMediaManager> _logger;

        public LocalFileSystemMediaManager(ILogger<LocalFileSystemMediaManager> logger)
        {
            _logger = logger;
        }

        public Task<bool> DeleteFileAsync(string filePath, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    Task.FromResult(true);
                }

                var fullPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "wwwroot", filePath));
                File.Delete(fullPath);

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Could not delete file '{filePath}'.");
                return Task.FromResult(false);
            }
        }

        public async Task<string> SaveFileAsync(Stream buffer, string originalFileName, string contentType, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!buffer.CanRead || !buffer.CanSeek || buffer.Length == 0) return null;

                var fileExt = Path.GetExtension(originalFileName).ToLower();
                var returnedFilePath = CreateFilePath(fileExt, contentType.ToLower());
                var fullPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "wwwroot", returnedFilePath));

                // Make sure we at at beginning of source stream
                buffer.Position = 0;

                await using var fileStream = new FileStream(fullPath, FileMode.Create);
                await buffer.CopyToAsync(fileStream, cancellationToken);

                return returnedFilePath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Could not save file '{originalFileName}'.");
                return null;
            }
        }

        private string CreateFilePath(string fileExt, string contentType = null)
        {
            return string.Format(PicturesFolder, Guid.NewGuid().ToString("N"), fileExt);
        }
    }
}
