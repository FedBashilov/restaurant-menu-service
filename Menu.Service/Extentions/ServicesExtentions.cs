// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Menu.Service.Extentions
{
    using Menu.Service;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public static class ServicesExtentions
    {
        public static void AddMenuServices(this IServiceCollection services)
        {
            services.TryAddSingleton<IMenuService, MenuService>();
        }
    }
}
