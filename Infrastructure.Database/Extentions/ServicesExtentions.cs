// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Database.Extentions
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServicesExtentions
    {
        public static void AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextFactory<MenuServiceDatabaseContext>(options =>
            {
                options.EnableSensitiveDataLogging();
                options.UseSqlServer(configuration.GetConnectionString("DatabaseConnection"),
                    options => options.MinBatchSize(5).MaxBatchSize(50));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
        }
    }
}
