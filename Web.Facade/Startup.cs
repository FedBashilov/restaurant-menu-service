// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade
{
    using Category.Service.Extentions;
    using CloudStorage.Service.Extentions;
    using Infrastructure.Auth.Extentions;
    using Infrastructure.Database.Extentions;
    using Menu.Service.Extentions;
    using Messaging.Service.Extentions;
    using Microsoft.OpenApi.Models;
    using Web.Facade.Middlewares;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthServices(this.Configuration);
            services.AddCloudStorageServices(this.Configuration);
            services.AddDatabaseServices(this.Configuration);
            services.AddMessagingServices(this.Configuration);
            services.AddMenuServices();
            services.AddCategoryServices();

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddControllers();

            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Menu API", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                });
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        },
                        Array.Empty<string>()
                    },
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            var supportedCultures = new[] { "en-US", "ru-RU" };
            var localizationOptions =
                new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            app.UseRequestLocalization(localizationOptions);

            app.UseMiddleware<HttpRequestBodyMiddleware>();
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
