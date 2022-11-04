namespace Devpack.Swagger.Extensions
{
    public class SwaggerSettings
    {
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string ContactName { get; set; } = default!;
        public string ContactEmail { get; set; } = default!;
        public string? ReadmeUrl { get; set; }
        public string OpenApiLicenseName { get; set; } = "MIT";
        public string OpenApiLicenseUrl { get; set; } = "https://opensource.org/licenses/MIT";
    }
}