using Devpack.Swagger.Extensions.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Devpack.Swagger.Extensions
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        public const string DocumentName = "v1";

        private readonly IHostEnvironment _hostEnviroment;
        private readonly SwaggerSettings _swaggerSettings;

        public ConfigureSwaggerOptions(SwaggerSettings settings, IHostEnvironment hostEnviroment)
        {
            _hostEnviroment = hostEnviroment;
            _swaggerSettings = settings;
        }

        public void Configure(SwaggerGenOptions options)
        {
            options.SwaggerDoc(DocumentName, CreateInfoForApiVersion());

            if (_hostEnviroment.IsProduction())
                return;

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Insira o token JWT desta maneira: Bearer {seu token}",
                Name = "Authorization",
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            foreach (var file in Directory.GetFiles(AppContext.BaseDirectory, "*.docs.xml", SearchOption.TopDirectoryOnly))
                options.IncludeXmlComments(file);

            options.EnableAnnotations();

            options.OperationFilter<SwaggerResponsesFilter>();
            options.ParameterFilter<SwaggerEnumsFilter>();
            options.OperationFilter<SwaggerPrivateSettersFilter>();
            options.OperationFilter<SwaggerBodyDescriptionFilter>();
        }

        private OpenApiInfo CreateInfoForApiVersion()
        {
            var info = new OpenApiInfo
            {
                Title = _swaggerSettings.Title,
                Version = "1.0",
                Description = _swaggerSettings.Description,
                Contact = new OpenApiContact { Name = $"- {_swaggerSettings.ContactName}", Email = _swaggerSettings.ContactEmail },
                License = new OpenApiLicense { Name = _swaggerSettings.OpenApiLicenseName, Url = new Uri(_swaggerSettings.OpenApiLicenseUrl) }
            };

            return info;
        }
    }
}