using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System;

namespace Devpack.Swagger.Extensions.Tests.Common.Factories
{
    public static class ModelMetadataFactory
    {
        public static ModelMetadata CreateDefaultMetadata()
        {
            return new FakeModelMetadata(ModelMetadataIdentity.ForType(typeof(Guid)));
        }

        public static ModelMetadata CreateNotReadOnlyMetadata()
        {
            var metadata = new FakeModelMetadata(ModelMetadataIdentity.ForType(typeof(Guid)));
            metadata.MockIsReadOnly(false);

            return metadata;
        }

        public static ModelMetadata CreateValidBindingSourceMetadata()
        {
            var metadata = new FakeModelMetadata(ModelMetadataIdentity.ForType(typeof(Guid)));
            metadata.MockBindingSource(new BindingSource(string.Empty, string.Empty, false, false));

            return metadata;
        }

        public static ModelMetadata CreateBodyMetadata()
        {
            var metadata = new FakeModelMetadata(ModelMetadataIdentity.ForType(typeof(Object2Test)));
            metadata.MockBindingSource(new BindingSource(string.Empty, string.Empty, false, false));

            return metadata;
        }
    }
}