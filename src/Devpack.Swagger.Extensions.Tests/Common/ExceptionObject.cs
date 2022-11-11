using System;

namespace Devpack.Swagger.Extensions.Tests.Common
{
    public class ExceptionObject
    {
        public ExceptionObject()
        {
            throw new InvalidOperationException();
        }
    }
}