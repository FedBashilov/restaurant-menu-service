// Copyright (c) Fedor Bashilov. All rights reserved.

namespace CloudStorage.Service
{
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using CloudStorage.Service.Exceptions;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class CloudStorageService : ICloudStorageService
    {
        private readonly CloudinarySettings cldSettings;
        private readonly Cloudinary cldStorage;
        private readonly ILogger<CloudStorageService> logger;

        public CloudStorageService(
            IOptions<CloudinarySettings> cldSettings,
            ILogger<CloudStorageService> logger)
        {
            this.cldSettings = cldSettings.Value;
            this.logger = logger;

            this.cldStorage = new Cloudinary(new Account(
                this.cldSettings.Cloud,
                this.cldSettings.ApiKey,
                this.cldSettings.ApiSecret));
            this.cldStorage.Api.Secure = true;
        }

        public async Task<Uri> UploadFile(byte[] file, string fileName, string folder)
        {
            await using var memoryStream = new MemoryStream();

            memoryStream.Write(file, 0, file.Length);
            memoryStream.Position = 0;

            var uploadparams = new ImageUploadParams
            {
                File = new FileDescription(fileName, memoryStream),
                Folder = folder,
            };

            var result = await this.cldStorage.UploadAsync(uploadparams);

            memoryStream.Close();

            if (result.Error != null)
            {
                this.logger.LogError($"Cloudinary error occured: {result.Error.Message}");
                throw new UploadFileException($"Cloudinary error occured: {result.Error.Message}");
            }

            return new Uri(result.SecureUrl.ToString());
        }
    }
}