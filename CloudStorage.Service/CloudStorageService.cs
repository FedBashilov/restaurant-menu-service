// Copyright (c) Fedor Bashilov. All rights reserved.

namespace CloudStorage.Service
{
    using System.IO;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using CloudStorage.Service.Exceptions;
    using CloudStorage.Service.Extentions;
    using CloudStorage.Service.Interfaces;
    using CloudStorage.Service.Settings;
    using Infrastructure.Core.Models;
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

        public async Task<CloudFile> UploadFile(byte[] file, string fileName, string folder)
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
                throw new UploadFileFailedException($"Cloudinary error occured: {result.Error.Message}");
            }

            var newFile = new CloudFile()
            {
                PublicId = result.PublicId,
                ResourceType = result.ResourceType,
                Url = result.SecureUrl.ToString(),
            };

            return newFile;
        }

        public async Task RemoveFile(string publicId, string fileType)
        {
            var uploadparams = new DeletionParams(publicId)
            {
                ResourceType = fileType.ToResourceType(),
            };

            var result = await this.cldStorage.DestroyAsync(uploadparams);

            if (result.Error != null)
            {
                this.logger.LogError($"Cloudinary error occured: {result.Error.Message}");
                throw new RemoveFileFailedException($"Cloudinary error occured: {result.Error.Message}");
            }
        }
    }
}