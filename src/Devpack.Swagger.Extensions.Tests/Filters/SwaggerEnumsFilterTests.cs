using Devpack.Swagger.Extensions.Filters;
using Devpack.Swagger.Extensions.Tests.Common;
using FluentAssertions;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Moq;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using Xunit;

namespace Devpack.Swagger.Extensions.Tests.Filters
{
    public class SwaggerEnumsFilterTests
    {
        private readonly Mock<ISchemaGenerator> _schemaGeneratorMock;

        public SwaggerEnumsFilterTests()
        {
            _schemaGeneratorMock = new Mock<ISchemaGenerator>();
        }

        [Fact(DisplayName = "Deve finalizar a execução quando o schema0 passado não for de um enum.")]
        public void Apply_Fail_WhenPropertyInfoNotIsEnum()
        {
            // Arrange
            var propertyDescription = Guid.NewGuid().ToString();

            var schema = new OpenApiSchema() { Description = propertyDescription };
            var context = new SchemaFilterContext(typeof(EnumTest), _schemaGeneratorMock.Object, new SchemaRepository());

            var filter = new SwaggerEnumsFilter();

            // Act
            filter.Apply(schema, context);

            // Asserts
            schema.Description.Should().Be(propertyDescription);
        }

        [Fact(DisplayName = "Deve ajustar a descrição do schema com os dados de um enum " +
            "quando um enum válido for passado.")]
        public void Apply_Success()
        {
            // Arrange
            var propertyDescription = Guid.NewGuid().ToString();

            var schema = new OpenApiSchema() { Description = propertyDescription };
            var context = new SchemaFilterContext(typeof(EnumTest), _schemaGeneratorMock.Object, new SchemaRepository());

            schema.Enum.Add(new OpenApiString("0"));

            var filter = new SwaggerEnumsFilter();

            // Act
            filter.Apply(schema, context);

            // Asserts
            schema.Description.Should().Be($"{propertyDescription}0 - Value1 | 1 - Value2 Formated");
        }
    }
}