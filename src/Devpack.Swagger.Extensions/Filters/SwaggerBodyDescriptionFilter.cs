using Devpack.Swagger.Extensions.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;

namespace Devpack.Swagger.Extensions.Filters
{
    public class SwaggerBodyDescriptionFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var bodyModel = context.ApiDescription.ParameterDescriptions?
                .FirstOrDefault(p => p.Source == BindingSource.Body)?.ModelMetadata.ModelType;

            if (bodyModel == null)
                return;

            var propertiesFilter = bodyModel.GetProperties()
                .Where(p => p.HasAttribute<SwaggerBodyParameterAttribute>());

            var bodyDescription = new StringBuilder();

            if (propertiesFilter.Any())
            {
                bodyDescription.Append("<h3>Body da Requisição</h3>");

                foreach (var property in propertiesFilter)
                {
                    bodyDescription.Append(
                        @$"<p>• {property.Name} : ""{property.GetCustomAttribute<SwaggerBodyParameterAttribute>()!.Description}""</p>");
                }
            }

            operation.Description += bodyDescription;
        }
    }
}