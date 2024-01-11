// Copyright (c) Fedor Bashilov. All rights reserved.

namespace CloudStorage.Service
{
    public interface ICloudStorageService
    {
        Task<Uri> UploadFile(byte[] file, string fileName, string folder);
    }
}