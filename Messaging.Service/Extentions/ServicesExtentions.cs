// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Messaging.Service.Extentions
{
    using Messaging.Service.Interfaces;
    using Messaging.Service.Settings;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public static class ServicesExtentions
    {
        public static void AddMessagingServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMqSettings"));
            services.TryAddSingleton<INewsMessagingService, NewsMessagingService>();
        }
    }
}
