using Devpack.Extensions.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Devpack.Swagger.Extensions.Filters
{
    public class SwaggerResponsesFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            RemoveSwaggerDefaultSchemas(operation.Responses);
            Process401Response(operation, context);
            Process400Response(operation, context);
            Process500Response(operation);
        }

        private static void Process401Response(OpenApiOperation operation, OperationFilterContext context)
        {
            string httpKey = StatusCodes.Status401Unauthorized.ToString();

            if (operation.Responses.ContainsKey(httpKey))
                return;

            if (CheckEndpointHasAthorizationAttribute(context.MethodInfo))
                operation.Responses.Add(httpKey, new OpenApiResponse { Description = "Unauthorized" });
        }

        private static void Process400Response(OpenApiOperation operation, OperationFilterContext context)
        {
            string httpKey = StatusCodes.Status400BadRequest.ToString();

            if (operation.Responses.ContainsKey(httpKey))
                return;

            if (CheckEndpointHasParameters(context))
                operation.Responses.Add(httpKey, new OpenApiResponse { Description = "Bad Request" });
        }

        private static void Process500Response(OpenApiOperation operation)
        {
            string httpKey = StatusCodes.Status500InternalServerError.ToString();

            if (operation.Responses.ContainsKey(httpKey))
                return;

            operation.Responses.Add(httpKey, new OpenApiResponse { Description = "Server Error" });
        }

        private static bool CheckEndpointHasParameters(OperationFilterContext context)
        {
            var hasParameter = context.MethodInfo.GetParameters().Any(p =>
                p.ParameterType.IsClass && p.ParameterType != typeof(string) && !p.CustomAttributes.Any(a => a.AttributeType == typeof(FromQueryAttribute))
                || p.CustomAttributes.Any(a => a.AttributeType == typeof(FromBodyAttribute)));

            return hasParameter;
        }

        private static bool CheckEndpointHasAthorizationAttribute(MethodInfo methodInfo)
        {
            return !methodInfo.HasAttribute<AllowAnonymousAttribute>()
                && (methodInfo.HasAttribute<AuthorizeAttribute>()
                || methodInfo.DeclaringType!.HasAttribute<AuthorizeAttribute>()
                || methodInfo.DeclaringType!.BaseType!.HasAttribute<AuthorizeAttribute>());
        }

        private static void RemoveSwaggerDefaultSchemas(OpenApiResponses responses)
        {
            if (responses == null)
                return;

            var responsesWithDefaultContent = responses.Values.Where(c =>
                c.Content.Any(c => c.Value.Schema.Reference?.Id == typeof(ProblemDetails).FullName));

            foreach (var response in responsesWithDefaultContent)
                response.Content = null;
        }
    }
}