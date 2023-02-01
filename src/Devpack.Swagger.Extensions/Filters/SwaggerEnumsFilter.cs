using Devpack.Extensions.Types;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Devpack.Swagger.Extensions.Filters
{
    public class SwaggerEnumsFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.Enum.IsNullOrEmpty())
                return;

            var enumTags = new List<string>();

            foreach (var enumItem in Enum.GetValues(context.Type))
                enumTags.Add($"{(int)enumItem!} - {(enumItem as Enum)?.GetDescription()}");

            schema.Description += string.Join(" | ", enumTags);
        }
    }
}