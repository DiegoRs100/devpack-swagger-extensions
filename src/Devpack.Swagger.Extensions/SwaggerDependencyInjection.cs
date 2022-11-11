using Devpack.Swagger.Extensions.Conventions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Diagnostics.CodeAnalysis;

namespace Devpack.Swagger.Extensions
{
    public static class SwaggerDependencyInjection
    {
        public static IServiceCollection AddSwaggerConfig(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
        {
            var swaggerSettings = services.AddSwaggerDefaultConfig(configuration, env);
            services.AddSwaggerGen(options => new ConfigureSwaggerOptions(swaggerSettings, env).Configure(options));

            return services;
        }

        public static IServiceCollection AddSwaggerConfig(this IServiceCollection services, IConfiguration configuration,
            IHostEnvironment env, Action<SwaggerGenOptions> swaggerOptions)
        {
            var swaggerSettings = services.AddSwaggerDefaultConfig(configuration, env);

            services.AddSwaggerGen(options =>
            {
                new ConfigureSwaggerOptions(swaggerSettings, env).Configure(options);
                swaggerOptions.Invoke(options);
            });

            return services;
        }

        [ExcludeFromCodeCoverage]
        public static IApplicationBuilder UseSwaggerDefaultConfig(this IApplicationBuilder app)
        {
            app.ExecuteSwaggerCommunConfig();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint($"./swagger/{ ConfigureSwaggerOptions.DocumentName }/swagger.json", "Api");
                options.InjectStylesheet("./wwwroot/assets/swagger.css");
                options.RoutePrefix = string.Empty;
                options.EnableFilter();
            });

            return app;
        }

        [ExcludeFromCodeCoverage]
        public static IApplicationBuilder UseSwaggerConfig(this IApplicationBuilder app, Action<SwaggerUIOptions> setupAction)
        {
            app.ExecuteSwaggerCommunConfig();
            app.UseSwaggerUI(setupAction);

            return app;
        }

        [ExcludeFromCodeCoverage]
        private static void ExecuteSwaggerCommunConfig(this IApplicationBuilder app)
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider($"{ AppContext.BaseDirectory }/Content"),
                RequestPath = "/wwwroot"
            });

            app.UseSwagger();
        }

        [ExcludeFromCodeCoverage]
        private static void ConfigureSwaggerDescription(SwaggerSettings swaggerSettings, IHostEnvironment env)
        {
            string swaggerDescriptionLayout = File.ReadAllText($"{ AppContext.BaseDirectory }/Content/assets/swagger.html");

            var disclaimerText = env.IsProduction()
                ? "OBS: Controllers information is not visible in production environment."
                : string.Empty;

            swaggerDescriptionLayout = swaggerDescriptionLayout
                .Replace("{{description}}", swaggerSettings.Description)
                .Replace("{{readme-url}}", swaggerSettings.ReadmeUrl)
                .Replace("{{enviroment-disclaimer}}", disclaimerText);

            swaggerSettings.Description = swaggerDescriptionLayout;
        }

        private static SwaggerSettings AddSwaggerDefaultConfig(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
        {
            if (env.IsProduction())
                services.AddMvcCore(options => options.Conventions.Add(new ControllersHideConvension()));

            var swaggerSettings = configuration.GetSection("Swagger")?.Get<SwaggerSettings>() ?? new SwaggerSettings();

            ConfigureSwaggerDescription(swaggerSettings, env);

            services.AddSingleton(swaggerSettings);

            return swaggerSettings;
        }
    }
}