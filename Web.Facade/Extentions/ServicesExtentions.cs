// Copyright (c) Fedor Bashilov. All rights reserved.

using Web.Facade.Services;

namespace Web.Facade.Extentions
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
