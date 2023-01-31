using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace Devpack.Swagger.Extensions.Tests.Common.Factories
{
    public static class OpenApiOperationFactory
    {
        public static OpenApiOperation CreateValid()
        {
            var operation = new OpenApiOperation();

            operation.Responses.Add("200", new OpenApiResponse { Description = "Success" });
            operation.Responses.Add("404", new OpenApiResponse { Description = "NotFound" });
            operation.Responses.Add("409", new OpenApiResponse { Description = "Conflict" });

            return operation;
        }

        public static OpenApiOperation CreateDefaultOpenApiOperation(
            IEnumerable<ApiParameterDescription> parametersDescription)
        {
            var operation = new OpenApiOperation()
            {
                Parameters = new List<OpenApiParameter>()
            };

            foreach (var description in parametersDescription)
                operation.Parameters.Add(new OpenApiParameter() { Name = description.Name });

            return operation;
        }
    }
}