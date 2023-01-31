using Devpack.Swagger.Extensions.Filters;
using Devpack.Swagger.Extensions.Tests.Common.Factories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using Xunit;

namespace Devpack.Swagger.Extensions.Tests.Filters
{
    public class SwaggerBodyDescriptionFilterTests
    {
        [Fact(DisplayName = "Não deve alterar a descrição do endpoint " +
            "quando não for identificado parâmetros no corpo da requisição.")]
        public void Apply_WhenNotBody()
        {
            // Arrange
            var context = new OperationFilterContext(new ApiDescription(), null, null, null);
            var filter = new SwaggerBodyDescriptionFilter();

            var operation = OpenApiOperationFactory.
                CreateDefaultOpenApiOperation(Array.Empty<ApiParameterDescription>());

            // Act
            filter.Apply(operation, context);

            // Asserts
            operation.Description.Should().BeNull();
        }

        [Fact(DisplayName = "Não deve alterar a descrição do endpoint " +
            "quando nenhum parâmetro do corpo possuir o atributo (SwaggerBodyDescription).")]
        public void Apply_WhenNotAttributes()
        {
            // Arrange
            var apiDescription = new ApiDescription();

            var operation = OpenApiOperationFactory.
                CreateDefaultOpenApiOperation(Array.Empty<ApiParameterDescription>());

            var bodyParameter = ApiParameterDescriptionFactory
                .CreateRequiredParameterDescription(ModelMetadataFactory.CreateDefaultMetadata());

            bodyParameter.Source = BindingSource.Body;

            apiDescription.ParameterDescriptions.Add(bodyParameter);

            var context = new OperationFilterContext(apiDescription, null, null, null);
            var filter = new SwaggerBodyDescriptionFilter();

            // Act
            filter.Apply(operation, context);

            // Asserts
            operation.Description.Should().BeEmpty();
        }

        [Fact(DisplayName = "Deve alterar a descrição do endpoint " +
            "quando ao menos um parâmetro do corpo possuir o atributo (SwaggerBodyDescription).")]
        public void Apply_Valid()
        {
            // Arrange
            var originalDescription = Guid.NewGuid().ToString();
            var apiDescription = new ApiDescription();

            var operation = OpenApiOperationFactory
                .CreateDefaultOpenApiOperation(Array.Empty<ApiParameterDescription>());

            var bodyParameter = ApiParameterDescriptionFactory
                .CreateRequiredParameterDescription(ModelMetadataFactory.CreateBodyMetadata());

            bodyParameter.Source = BindingSource.Body;
            operation.Description = originalDescription;

            apiDescription.ParameterDescriptions.Add(bodyParameter);

            var context = new OperationFilterContext(apiDescription, null, null, null);
            var filter = new SwaggerBodyDescriptionFilter();

            // Act
            filter.Apply(operation, context);

            // Asserts
            operation.Description.Should().Be($"{originalDescription}<h3>Body da Requisição</h3><p>• Property2 : \"Teste Descrição 1\"</p><p>• Property3 : \"Teste Descrição 2\"</p>");
        }
    }
}