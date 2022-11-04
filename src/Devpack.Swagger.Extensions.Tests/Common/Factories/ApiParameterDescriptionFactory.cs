using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System;

namespace Devpack.Swagger.Extensions.Tests.Common.Factories
{
    public static class ApiParameterDescriptionFactory
    {
        public static ApiParameterDescription CreateRequiredParameterDescription(ModelMetadata modelMetadata)
        {
            return new ApiParameterDescription()
            {
                Name = Guid.NewGuid().ToString(),
                IsRequired = true,
                ModelMetadata = modelMetadata
            };
        }

        public static ApiParameterDescription CreateNotRequiredParameterDescription()
        {
            return new ApiParameterDescription()
            {
                Name = Guid.NewGuid().ToString(),
                ModelMetadata = new FakeModelMetadata(ModelMetadataIdentity.ForType(typeof(EnumTest)))
            };
        }
    }
}