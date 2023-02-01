using Bogus;
using Devpack.Swagger.Extensions.Filters;
using FluentAssertions;
using Microsoft.Extensions.Hosting;
using Moq;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using Xunit;

namespace Devpack.Swagger.Extensions.Tests
{
    public class ConfigureSwaggerOptionsTests
    {
        private readonly Mock<IHostEnvironment> _hostEnvironmentMock;

        public ConfigureSwaggerOptionsTests()
        {
            _hostEnvironmentMock = new Mock<IHostEnvironment>();
        }

        [Theory(DisplayName = "Deve configurar corretamente os filtros de parâmetros e operação quando o ambiente for diferente de produção.")]
        [InlineData("Sandbox")]
        [InlineData("Development")]
        public void Configure_Filters(string enviromentKey)
        {
            // Arrange
            _hostEnvironmentMock.SetupGet(m => m.EnvironmentName).Returns(enviromentKey);

            var genOptions = new SwaggerGenOptions();
            var configureSwaggerOptions = new ConfigureSwaggerOptions(new SwaggerSettings(), _hostEnvironmentMock.Object);

            // Act
            configureSwaggerOptions.Configure(genOptions);

            // Asserts
            genOptions.ParameterFilterDescriptors.Should().HaveCount(1);
            genOptions.OperationFilterDescriptors.Should().HaveCount(3);
            genOptions.SchemaFilterDescriptors.Should().HaveCount(2);
            genOptions.ParameterFilterDescriptors.Should().Contain(p => p.Type == typeof(AnnotationsParameterFilter));
            genOptions.SchemaFilterDescriptors.Should().Contain(p => p.Type == typeof(SwaggerEnumsFilter));
            genOptions.OperationFilterDescriptors.Should().Contain(p => p.Type == typeof(AnnotationsOperationFilter));
            genOptions.OperationFilterDescriptors.Should().Contain(p => p.Type == typeof(SwaggerResponsesFilter));
        }

        [Theory(DisplayName = "Deve criar corretamente o objeto (OpenApiInfo) quando o o ambiente for diferente de produção.")]
        [InlineData("Sandbox")]
        [InlineData("Development")]
        [InlineData("Production")]
        public void Configure_OpenApiInfo(string enviromentKey)
        {
            // Arrange
            _hostEnvironmentMock.SetupGet(m => m.EnvironmentName).Returns(enviromentKey);

            var genOptions = new SwaggerGenOptions();
            var customOptions = CreateDefaultSwaggerSettings();
            var configureSwaggerOptions = new ConfigureSwaggerOptions(customOptions, _hostEnvironmentMock.Object);

            // Act
            configureSwaggerOptions.Configure(genOptions);
            var swaggerInfo = genOptions.SwaggerGeneratorOptions.SwaggerDocs[ConfigureSwaggerOptions.DocumentName];

            // Asserts
            swaggerInfo.Title.Should().Be(customOptions.Title);
            swaggerInfo.Description.Should().Be(customOptions.Description);
            swaggerInfo.Contact.Name.Should().Be($"- {customOptions.ContactName}");
            swaggerInfo.Contact.Email.Should().Be(customOptions.ContactEmail);
            swaggerInfo.License.Name.Should().Be(customOptions.OpenApiLicenseName);
            swaggerInfo.License.Url.Should().Be(new Uri(customOptions.OpenApiLicenseUrl));
        }

        [Fact(DisplayName = "Não deve habilitar os filtros quando o abiente for de produção.")]
        public void Configure_WhenProduction()
        {
            // Arrange
            _hostEnvironmentMock.SetupGet(m => m.EnvironmentName).Returns("Production");

            var genOptions = new SwaggerGenOptions();
            var configureSwaggerOptions = new ConfigureSwaggerOptions(new SwaggerSettings(), _hostEnvironmentMock.Object);

            // Act
            configureSwaggerOptions.Configure(genOptions);

            // Asserts
            genOptions.ParameterFilterDescriptors.Should().BeEmpty();
            genOptions.OperationFilterDescriptors.Should().BeEmpty();
        }

        private static SwaggerSettings CreateDefaultSwaggerSettings()
        {
            var faker = new Faker("pt_BR");

            return new SwaggerSettings()
            {
                Title = faker.Random.Word(),
                Description = faker.Random.Words(10),
                ContactName = faker.Person.FullName,
                ContactEmail = faker.Person.Email,
                OpenApiLicenseName = faker.Random.Word(),
                OpenApiLicenseUrl = faker.Internet.Url()
            };
        }
    }
}