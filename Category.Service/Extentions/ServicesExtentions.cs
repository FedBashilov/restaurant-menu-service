// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Category.Service.Extentions
{
    using Category.Service;
    using Category.Service.Interfaces;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public static class ServicesExtentions
    {
        public static void AddCategoryServices(this IServiceCollection services)
        {
            services.TryAddSingleton<ICategoryService, CategoryService>();
        }
    }
}
