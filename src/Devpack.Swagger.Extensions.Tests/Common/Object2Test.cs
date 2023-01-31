using Devpack.Swagger.Extensions.Attributes;
using Microsoft.AspNetCore.Authorization;

namespace Devpack.Swagger.Extensions.Tests.Common
{
    public class Object2Test
    {
        public string? Property1 { get; set; }

        [SwaggerBodyParameter("Teste Descrição 1")]
        public string? Property2 { get; set; }

        [SwaggerBodyParameter("Teste Descrição 2")]
        public string? Property3 { get; set; }

        [Authorize]
        public static void Method1()
        {
            // Necessário para realização de testes unitários
        }

        public static void Method2()
        {
            // Necessário para realização de testes unitários
        }
    }
}