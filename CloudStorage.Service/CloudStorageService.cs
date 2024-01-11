// Copyright (c) Fedor Bashilov. All rights reserved.

namespace CloudStorage.Service
{
    using Dropbox.Api;
    using Dropbox.Api.Files;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class CloudStorageService : ICloudStorageService
    {
        private readonly DropboxSettings drxSettings;
        private readonly ILogger<CloudStorageService> logger;

        public CloudStorageService(
            IOptions<DropboxSettings> drxSettings,
            ILogger<CloudStorageService> logger)
        {
            this.drxSettings = drxSettings.Value;
            this.logger = logger;
        }

        public async Task<Uri> UploadFile(byte[] file, string fileName, string folderName)
        {
            using var dbx = new DropboxClient(this.drxSettings.Token);

            using var mem = new MemoryStream(file);

            var updated = await dbx.Files.UploadAsync(
                $"/{folderName}/{fileName}",
                WriteMode.Overwrite.Instance,
                body: mem);

            this.logger.LogInformation($"Uploaded {fileName} to Dropbox.");

            var tx = await dbx.Sharing.CreateSharedLinkWithSettingsAsync($"/{folderName}/{fileName}");

            return new Uri(tx.Url);
        }
    }
}