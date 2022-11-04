using Devpack.Swagger.Extensions.Filters;
using Devpack.Swagger.Extensions.Tests.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Xunit;

namespace Devpack.Swagger.Extensions.Tests.Filters
{
    public class SwaggerEnumsFilterTests
    {
        [Fact(DisplayName = "Deve finalizar a execução quando a propertyInfo passada não for de um enum.")]
        [Trait("Category", "Filters")]
        public void Apply_Fail_WhenPropertyInfoNotIsEnum()
        {
            var options = new OpenApiParameter();
            var modelMetadataMock = new FakeModelMetadata(ModelMetadataIdentity.ForType(typeof(EnumTest)));

            var apiParameterDescription = new ApiParameterDescription()
            {
                ModelMetadata = modelMetadataMock
            };

            var context = new ParameterFilterContext(apiParameterDescription, null, null, null, null);

            var filter = new SwaggerEnumsFilter();
            filter.Apply(options, context);

            options.Description.Should().BeNull();
        }

        [Fact(DisplayName = "Deve ajustar a descrição do objeto (OpenApiParameter) com os dados de um enum " +
            "quando um enum válido for passado.")]
        [Trait("Category", "Filters")]
        public void Apply_Success()
        {
            var options = new OpenApiParameter();

            var modelMetadataMock = new FakeModelMetadata(ModelMetadataIdentity.ForType(typeof(EnumTest)));
            modelMetadataMock.MockIsEnum(true);

            var apiParameterDescription = new ApiParameterDescription()
            {
                ModelMetadata = modelMetadataMock
            };

            var context = new ParameterFilterContext(apiParameterDescription, null, null, null, null);

            var filter = new SwaggerEnumsFilter();
            filter.Apply(options, context);

            options.Description.Should().Be("0 - Value1 | 1 - Value2 Formated");
        }
    }
}