using Devpack.Extensions.Types;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Devpack.Swagger.Extensions.Filters
{
    public class SwaggerEnumsFilter : IParameterFilter
    {
        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            if (!context.ApiParameterDescription.ModelMetadata.IsEnum)
                return;

            var type = context.ApiParameterDescription.ModelMetadata.ModelType;

            var enumTags = new List<string>();

            foreach (var enumItem in Enum.GetValues(type))
                enumTags.Add($"{(int)enumItem!} - { (enumItem as Enum)?.GetDescription() }");

            parameter.Description = string.Join(" | ", enumTags);
        }
    }
}