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
        private readonly CloudinarySettings cloudinarySettings;
        private readonly Cloudinary cloudinary;
        private readonly ILogger<CloudStorageService> logger;

        public CloudStorageService(
            IOptions<CloudinarySettings> cloudinarySettings,
            ILogger<CloudStorageService> logger)
        {
            this.cloudinarySettings = cloudinarySettings.Value;
            this.logger = logger;

            var account = new Account(
                this.cloudinarySettings.Cloud,
                this.cloudinarySettings.ApiKey,
                this.cloudinarySettings.ApiSecret);
            this.cloudinary = new Cloudinary(account);
            this.cloudinary.Api.Secure = true;
        }

        public async Task<Uri> UploadFile(byte[] file, string fileName, string folder)
        {
            using var memoryStream = new MemoryStream();
            memoryStream.Write(file, 0, file.Length);
            memoryStream.Position = 0;

            var uploadparams = new ImageUploadParams
            {
                File = new FileDescription(fileName, memoryStream),
                Folder = folder,
            };

            var result = await this.cloudinary.UploadAsync(uploadparams);

            if (result.Error != null)
            {
                this.logger.LogError($"Cloudinary error occured: {result.Error.Message}");
                throw new UploadFileException($"Cloudinary error occured: {result.Error.Message}");
            }

            return new Uri(result.SecureUrl.ToString());
        }
    }
}