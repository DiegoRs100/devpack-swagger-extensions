using Devpack.Swagger.Extensions.Filters;
using Devpack.Swagger.Extensions.Tests.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using Xunit;

namespace Devpack.Swagger.Extensions.Tests.Filters
{
    public class SwaggerResponsesFilterTests
    {
        [Fact(DisplayName = "Deve remover os schemas defalt do swagger quando o método for chamado")]
        public void Apply_RemoveDefaultSchemas()
        {
            var schemaKey = Guid.NewGuid().ToString();
            var contentKey1 = Guid.NewGuid().ToString();
            var contentKey2 = Guid.NewGuid().ToString();

            var metodInfo = typeof(ObjectTest).GetMethod(nameof(ObjectTest.Method1));

            var contentMock = new Dictionary<string, OpenApiMediaType>
            {
                {
                    schemaKey, new OpenApiMediaType()
                    {
                        Schema = new OpenApiSchema()
                        {
                            Reference = new OpenApiReference() { Id = typeof(ProblemDetails).FullName }
                        }
                    }
                }
            };

            var operation = new OpenApiOperation();
            operation.Responses.Add(contentKey1, new OpenApiResponse() { Description = contentKey1, Content = contentMock });
            operation.Responses.Add(contentKey2, new OpenApiResponse() { Description = contentKey2, Content = contentMock });

            var context = new OperationFilterContext(null, null, null, metodInfo);
            var filter = new SwaggerResponsesFilter();

            filter.Apply(operation, context);

            operation.Responses[contentKey1].Content.Should().BeNull();
            operation.Responses[contentKey2].Content.Should().BeNull();
        }

        [Theory(DisplayName = "Não deve criar as response infos dos códigos correspondentes " +
            "quando o atrrubute (ProducesResponseTypeAttribute) não existir no método.")]
        [InlineData("200")]
        [InlineData("404")]
        [InlineData("409")]
        public void Apply_WhenNotHasProducesResponseTypeAttribute(string httpCode)
        {
            var metodInfo = typeof(ObjectTest).GetMethod(nameof(ObjectTest.Method1));

            var operation = new OpenApiOperation();
            var context = new OperationFilterContext(null, null, null, metodInfo);

            var filter = new SwaggerResponsesFilter();
            filter.Apply(operation, context);

            operation.Responses.Should().NotContainKey(httpCode);
        }

        [Fact(DisplayName = "Deve criar um response info informado o retorno 400 quando o método analisado tiver parâmetros do tipo (class).")]
        public void Apply_WhenMethodHasClassParameters()
        {
            var metodInfo = typeof(ObjectTest).GetMethod(nameof(ObjectTest.Method3));

            var operation = new OpenApiOperation();
            var context = new OperationFilterContext(null, null, null, metodInfo);

            var filter = new SwaggerResponsesFilter();
            filter.Apply(operation, context);

            operation.Responses.Should().Contain(r => r.Key == "400" && r.Value.Description == "Bad Request");
        }

        [Fact(DisplayName = "Deve criar um response info informado o retorno 400 quando o método analisado tiver parâmetros no corpo com o atributo (FromBody).")]
        public void Apply_WhenMethodHasParametersFromBodyAttribute()
        {
            var metodInfo = typeof(ObjectTest).GetMethod(nameof(ObjectTest.Method4));

            var operation = new OpenApiOperation();
            var context = new OperationFilterContext(null, null, null, metodInfo);

            var filter = new SwaggerResponsesFilter();
            filter.Apply(operation, context);

            operation.Responses.Should().Contain(r => r.Key == "400" && r.Value.Description == "Bad Request");
        }

        [Fact(DisplayName = "Não deve criar um response info informado o retorno 400 quando o método analisado não tiver parâmetros.")]
        public void Apply_WhenMethodHasNotParameters()
        {
            var metodInfo = typeof(ObjectTest).GetMethod(nameof(ObjectTest.Method1));

            var operation = new OpenApiOperation();
            var context = new OperationFilterContext(null, null, null, metodInfo);

            var filter = new SwaggerResponsesFilter();
            filter.Apply(operation, context);

            operation.Responses.Should().NotContainKey("400");
        }

        [Fact(DisplayName = "Não deve criar um response info informado o retorno 400 quando o método analisado tiver apenas parâmetros do tipo (class) com o atributo (FromQuery).")]
        public void Apply_WhenMethodHasParametersFromQueryAttribute()
        {
            var metodInfo = typeof(ObjectTest).GetMethod(nameof(ObjectTest.Method5));

            var operation = new OpenApiOperation();
            var context = new OperationFilterContext(null, null, null, metodInfo);

            var filter = new SwaggerResponsesFilter();
            filter.Apply(operation, context);

            operation.Responses.Should().NotContainKey("400");
        }

        [Fact(DisplayName = "Não deve criar um response info informado o retorno 400 quando o método analisado tiver apenas parâmetros primitivos.")]
        public void Apply_WhenMethodHasOnlyPrimitivesParameters()
        {
            var metodInfo = typeof(ObjectTest).GetMethod(nameof(ObjectTest.Method6));

            var operation = new OpenApiOperation();
            var context = new OperationFilterContext(null, null, null, metodInfo);

            var filter = new SwaggerResponsesFilter();
            filter.Apply(operation, context);

            operation.Responses.Should().NotContainKey("400");
        }

        [Fact(DisplayName = "Não deve criar um novo response info informado o retorno 400 quando o response info 400 já existir.")]
        public void Apply_WhenResponse400AlreadyExists()
        {
            var metodInfo = typeof(ObjectTest).GetMethod(nameof(ObjectTest.Method4));

            var operation = new OpenApiOperation();
            operation.Responses.Add("400", new OpenApiResponse() { Description = "Error 400" });

            var context = new OperationFilterContext(null, null, null, metodInfo);
            var filter = new SwaggerResponsesFilter();

            filter.Invoking(f => f.Apply(operation, context)).Should().NotThrow();
        }

        [Fact(DisplayName = "Deve criar um response info informado o retorno 401 quando a classe analisada tiver o atributo (Authorize).")]
        public void Apply_WhenClassHasAuthorizeAttribute()
        {
            var metodInfo = typeof(ObjectTest).GetMethod(nameof(ObjectTest.Method1));

            var operation = new OpenApiOperation();
            var context = new OperationFilterContext(null, null, null, metodInfo);

            var filter = new SwaggerResponsesFilter();
            filter.Apply(operation, context);

            operation.Responses.Should().Contain(r => r.Key == "401" && r.Value.Description == "Unauthorized");
        }

        [Fact(DisplayName = "Deve criar um response info informado o retorno 401 quando o método analisado tiver o atributo (Authorize).")]
        public void Apply_WhenMethodHasAuthorizeAttribute()
        {
            var metodInfo = typeof(Object2Test).GetMethod(nameof(Object2Test.Method1));

            var operation = new OpenApiOperation();
            var context = new OperationFilterContext(null, null, null, metodInfo);

            var filter = new SwaggerResponsesFilter();
            filter.Apply(operation, context);

            operation.Responses.Should().Contain(r => r.Key == "401" && r.Value.Description == "Unauthorized");
        }

        [Fact(DisplayName = "Não deve criar um response info informado o retorno 401 quando a classe e o método analisado não tiverem o atributo (Authorize).")]
        public void Apply_WhenClassAndMethodHasNotAuthorizeAttribute()
        {
            var metodInfo = typeof(Object2Test).GetMethod(nameof(Object2Test.Method2));

            var operation = new OpenApiOperation();
            var context = new OperationFilterContext(null, null, null, metodInfo);

            var filter = new SwaggerResponsesFilter();
            filter.Apply(operation, context);

            operation.Responses.Should().NotContainKey("401");
        }

        [Fact(DisplayName = "Não deve criar um response info informado o retorno 401 quando o método analisado tiver o atributo (AllowAnonymous).")]
        public void Apply_WhenMethodHasAllowAnonymousAttribute()
        {
            var metodInfo = typeof(ObjectTest).GetMethod(nameof(ObjectTest.Method3));

            var operation = new OpenApiOperation();
            var context = new OperationFilterContext(null, null, null, metodInfo);

            var filter = new SwaggerResponsesFilter();
            filter.Apply(operation, context);

            operation.Responses.Should().NotContainKey("401");
        }

        [Fact(DisplayName = "Não deve criar um novo response info informado o retorno 401 quando o response info 401 já existir.")]
        public void Apply_WhenResponse401AlreadyExists()
        {
            var methodInfo = typeof(ObjectTest).GetMethod(nameof(ObjectTest.Method4));

            var operation = new OpenApiOperation();
            operation.Responses.Add("401", new OpenApiResponse() { Description = "Error 401" });

            var context = new OperationFilterContext(null, null, null, methodInfo);
            var filter = new SwaggerResponsesFilter();

            filter.Invoking(f => f.Apply(operation, context)).Should().NotThrow();
        }

        [Fact(DisplayName = "Não deve criar um novo response info informado o retorno 500 quando o response info 500 já existir.")]
        public void Apply_WhenResponse500AlreadyExists()
        {
            var metodInfo = typeof(ObjectTest).GetMethod(nameof(ObjectTest.Method1));

            var operation = new OpenApiOperation();
            operation.Responses.Add("500", new OpenApiResponse() { Description = "Error 500" });

            var context = new OperationFilterContext(null, null, null, metodInfo);
            var filter = new SwaggerResponsesFilter();

            filter.Invoking(f => f.Apply(operation, context)).Should().NotThrow();
        }
    }
}