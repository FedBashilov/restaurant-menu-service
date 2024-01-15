// Copyright (c) Fedor Bashilov. All rights reserved.

namespace CloudStorage.Service.Extentions
{
    using CloudStorage.Service.Interfaces;
    using CloudStorage.Service.Settings;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public static class ServicesExtentions
    {
        public static void AddCloudStorageServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));
            services.TryAddSingleton<ICloudStorageService, CloudStorageService>();
        }
    }
}
