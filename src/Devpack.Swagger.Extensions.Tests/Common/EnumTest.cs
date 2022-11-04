using System.ComponentModel;

namespace Devpack.Swagger.Extensions.Tests.Common
{
    public enum EnumTest
    {
        Value1 = 0,

        [Description("Value2 Formated")]
        Value2 = 1
    }
}