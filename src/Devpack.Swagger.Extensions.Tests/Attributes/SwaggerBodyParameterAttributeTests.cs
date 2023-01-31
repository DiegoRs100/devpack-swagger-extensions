using Devpack.Swagger.Extensions.Attributes;
using FluentAssertions;
using System;
using Xunit;

namespace Devpack.Swagger.Extensions.Tests.Attributes
{
    public class SwaggerBodyParameterAttributeTests
    {
        [Fact(DisplayName = "Deve inicializar corretamente a propriedade (Description) quando o objeto for instanciado.")]
        public void Constructor() 
        {
            var description = Guid.NewGuid().ToString();
            var attribute = new SwaggerBodyParameterAttribute(description);

            attribute.Description.Should().Be(description);
        }
    }
}
