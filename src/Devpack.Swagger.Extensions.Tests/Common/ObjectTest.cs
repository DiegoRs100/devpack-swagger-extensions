using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Devpack.Swagger.Extensions.Tests.Common
{
    [Authorize]
    public class ObjectTest
    {
        public EnumTest Property1 { get; set; }
        public Guid Property2 { get; set; }
        public string? Property3 { get; set; }

        public static void Method1()
        {
            // Necessário para realização de testes unitários
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public static void Method2()
        {
            // Necessário para realização de testes unitários
        }

        [AllowAnonymous]
        public static void Method3(Object2Test object2)
        {
            object2.Property1 = string.Empty;
        }

        public static int Method4([FromBody] int object2)
        {
            return object2;
        }

        public static void Method5([FromQuery] Object2Test object2)
        {
            object2.Property1 = string.Empty;
        }

        public static string Method6(int parameter1, string parameter2, bool parameter3)
        {
            return $"{ parameter1 }/{ parameter2 }/{ parameter3 }";
        }
    }
}