using Devpack.Extensions.Types;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Devpack.Swagger.Extensions.Filters
{
    public class SwaggerPrivateSettersFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription?.ParameterDescriptions == null)
                return;

            var parametersReadyOnly = context.ApiDescription.ParameterDescriptions.Where(pd =>
                pd.ModelMetadata.IsReadOnly && !pd.IsRequired && pd.ModelMetadata.BindingSource == null);

            foreach (var parameterToHide in parametersReadyOnly)
            {
                var parameter = operation.Parameters.First(p => p.Name.Match(parameterToHide.Name));
                operation.Parameters.Remove(parameter);
            }
        }
    }
}