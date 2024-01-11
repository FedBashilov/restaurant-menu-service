// Copyright (c) Fedor Bashilov. All rights reserved.

namespace CloudStorage.Service.Extentions
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public static class ServicesExtentions
    {
        public static void AddCloudStorageServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DropboxSettings>(configuration.GetSection("DropboxSettings"));
            services.TryAddSingleton<ICloudStorageService, CloudStorageService>();
        }
    }
}
