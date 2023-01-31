using Bogus;
using Devpack.Swagger.Extensions.Tests.Common;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Moq;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Text;
using Xunit;

namespace Devpack.Swagger.Extensions.Tests
{
    public class SwaggerDependencyInjectionTests
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IHostEnvironment> _hostEnvironmentMock;
        private readonly Faker _faker;

        public SwaggerDependencyInjectionTests()
        {
            _configurationMock = new Mock<IConfiguration>();
            _hostEnvironmentMock = new Mock<IHostEnvironment>();
            _faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "Deve configurar a injeção dos serviços do swagger quando o método (AddSwaggerConfig) for chamado.")]
        public void AddSwaggerConfig()
        {
            _hostEnvironmentMock.SetupGet(m => m.EnvironmentName).Returns("Sandbox");

            var services = new ServiceCollection();
            services.AddSwaggerConfig(_configurationMock.Object, _hostEnvironmentMock.Object);

            var serviceProvider = services.BuildServiceProvider();

            var schemaGenerator = serviceProvider.GetService<ISchemaGenerator>();
            var injectedOptions = serviceProvider.GetService<SwaggerSettings>();
            var swaggerGenOptions = serviceProvider.GetService<IConfigureOptions<SwaggerGenOptions>>();

            schemaGenerator.Should().NotBeNull();
            injectedOptions.Should().NotBeNull();
            swaggerGenOptions.Should().NotBeNull();
        }

        [Fact(DisplayName = "Deve realizar as devidas substituições no layout de descrição quando o método (AddSwaggerConfig) for chamado.")]
        public void AddSwaggerConfig_Description()
        {
            _hostEnvironmentMock.SetupGet(m => m.EnvironmentName).Returns("Sandbox");

            var services = new ServiceCollection();

            var description = _faker.Random.Words(10);
            var readmeUrl = _faker.Internet.Url();

            var appSettings = @$"
                {{
                    ""Swagger"": {{
                        ""Description"" : ""{description}"",
                        ""ReadmeUrl"" : ""{readmeUrl}"",
                    }}
                }}";

            var configuration = new ConfigurationBuilder()
                .AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(appSettings)))
                .Build();

            services.AddSwaggerConfig(configuration, _hostEnvironmentMock.Object);

            var serviceProvider = services.BuildServiceProvider();
            var injectedOptions = serviceProvider.GetService<SwaggerSettings>();

            injectedOptions?.Description.Should()
                .Contain("<h4>Documentation</h4>")
                .And.Contain(description)
                .And.Contain(readmeUrl);
        }

        [Fact(DisplayName = "Deve configurar a injeção dos serviços do swagger quando o método (AddSwaggerConfig) for chamado " +
            "passando options customizadas.")]
        public void AddSwaggerConfig_UsingCustomOptions()
        {
            _hostEnvironmentMock.SetupGet(m => m.EnvironmentName).Returns("Sandbox");

            var services = new ServiceCollection();

            services.AddSwaggerConfig(_configurationMock.Object, _hostEnvironmentMock.Object, options =>
                options.Equals(new ExceptionObject()));

            var serviceProvider = services.BuildServiceProvider();

            var injectedOptions = serviceProvider.GetService<SwaggerSettings>();
            var swaggerGenOptions = serviceProvider.GetService<IConfigureOptions<SwaggerGenOptions>>();

            serviceProvider.Invoking(sp => sp.GetService<ISchemaGenerator>())
                .Should().Throw<InvalidOperationException>();

            injectedOptions.Should().NotBeNull();
            swaggerGenOptions.Should().NotBeNull();
        }
    }
}