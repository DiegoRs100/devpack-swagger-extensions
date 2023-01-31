using Devpack.Swagger.Extensions.Filters;
using Devpack.Swagger.Extensions.Tests.Common;
using Devpack.Swagger.Extensions.Tests.Common.Factories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Swashbuckle.AspNetCore.SwaggerGen;
using Xunit;

namespace Devpack.Swagger.Extensions.Tests.Filters
{
    public class SwaggerPrivateSettersFilterTests
    {
        [Fact(DisplayName = "Não deve lançar exception quando o (ParameterDescriptions) for nulo.")]
        public void Apply_WhenParameterDescriptionsNull()
        {
            var context = new OperationFilterContext(null, null, null, null);
            var filter = new SwaggerPrivateSettersFilter();

            filter.Invoking(f => f.Apply(null!, context)).Should().NotThrow();
        }

        [Fact(DisplayName = "Não deve lançar exception quando a operação não possuir parâmetros.")]
        public void Apply_WhenNotParameters()
        {
            var context = new OperationFilterContext(new ApiDescription(), null, null, null);
            var filter = new SwaggerPrivateSettersFilter();

            filter.Invoking(f => f.Apply(null!, context)).Should().NotThrow();
        }

        [Fact(DisplayName = "Deve remover os parâmetros do tipo (ReadOnly) quando o método for chamado.")]
        public void Apply_ReadOnlyParameters()
        {
            // Arrange
            var metodInfo = typeof(ObjectTest).GetMethod(nameof(ObjectTest.Method1));
            var apiDescription = new ApiDescription();

            var invalidParameter = ApiParameterDescriptionFactory.CreateNotRequiredParameterDescription();

            var validParameter1 = ApiParameterDescriptionFactory
                .CreateRequiredParameterDescription(ModelMetadataFactory.CreateDefaultMetadata());

            var validParameter2 = ApiParameterDescriptionFactory
                .CreateRequiredParameterDescription(ModelMetadataFactory.CreateNotReadOnlyMetadata());

            var validParameter3 = ApiParameterDescriptionFactory
                .CreateRequiredParameterDescription(ModelMetadataFactory.CreateValidBindingSourceMetadata());

            apiDescription.ParameterDescriptions.Add(invalidParameter);
            apiDescription.ParameterDescriptions.Add(validParameter1);
            apiDescription.ParameterDescriptions.Add(validParameter2);
            apiDescription.ParameterDescriptions.Add(validParameter3);

            var operation = OpenApiOperationFactory
                .CreateDefaultOpenApiOperation(apiDescription.ParameterDescriptions);

            var context = new OperationFilterContext(apiDescription, null, null, metodInfo);
            var filter = new SwaggerPrivateSettersFilter();

            // Act
            filter.Apply(operation, context);

            // Asserts
            operation.Parameters.Should().HaveCount(3);
            operation.Parameters.Should().NotContain(p => p.Name == invalidParameter.Name);
        }
    }
}