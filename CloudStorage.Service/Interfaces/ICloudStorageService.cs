// Copyright (c) Fedor Bashilov. All rights reserved.

namespace CloudStorage.Service.Interfaces
{
    using Infrastructure.Core.Models;

    public interface ICloudStorageService
    {
        Task<CloudFile> UploadFile(byte[] file, string fileName, string folder);

        Task RemoveFile(string publicId, string fileType);
    }
}