using Microsoft.OpenApi.Models;

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
    }
}