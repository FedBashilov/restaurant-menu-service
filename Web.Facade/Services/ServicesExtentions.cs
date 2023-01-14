// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Services
{
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public static class ServicesExtentions
    {
        public static void AddAdminServices(this IServiceCollection services)
        {
            services.TryAddSingleton<IMenuService, MenuService>();
        }
    }
}
