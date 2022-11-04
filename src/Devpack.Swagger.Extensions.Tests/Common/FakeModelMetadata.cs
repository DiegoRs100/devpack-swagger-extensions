using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System;
using System.Collections.Generic;

namespace Devpack.Swagger.Extensions.Tests.Common
{
    public class FakeModelMetadata : ModelMetadata
    {
        private bool _isEnum;
        private bool _isReadOnly = true;
        private BindingSource? _bindingSource;

        public override IReadOnlyDictionary<object, object> AdditionalValues => throw new NotImplementedException();

        public override ModelPropertyCollection Properties => throw new NotImplementedException();

        public override string? BinderModelName => throw new NotImplementedException();

        public override Type? BinderType => throw new NotImplementedException();

        public override BindingSource? BindingSource => _bindingSource;

        public override bool ConvertEmptyStringToNull => throw new NotImplementedException();

        public override string? DataTypeName => throw new NotImplementedException();

        public override string? Description => throw new NotImplementedException();

        public override string? DisplayFormatString => throw new NotImplementedException();

        public override string? DisplayName => throw new NotImplementedException();

        public override string? EditFormatString => throw new NotImplementedException();

        public override ModelMetadata? ElementMetadata => throw new NotImplementedException();

        public override IEnumerable<KeyValuePair<EnumGroupAndName, string>>? EnumGroupedDisplayNamesAndValues => throw new NotImplementedException();

        public override IReadOnlyDictionary<string, string>? EnumNamesAndValues => throw new NotImplementedException();

        public override bool HasNonDefaultEditFormat => throw new NotImplementedException();

        public override bool HtmlEncode => throw new NotImplementedException();

        public override bool HideSurroundingHtml => throw new NotImplementedException();

        public override bool IsBindingAllowed => throw new NotImplementedException();

        public override bool IsBindingRequired => throw new NotImplementedException();

        public override bool IsEnum => _isEnum;

        public override bool IsFlagsEnum => throw new NotImplementedException();

        public override bool IsReadOnly => _isReadOnly;

        public override bool IsRequired => throw new NotImplementedException();

        public override ModelBindingMessageProvider ModelBindingMessageProvider => throw new NotImplementedException();

        public override int Order => throw new NotImplementedException();

        public override string? Placeholder => throw new NotImplementedException();

        public override string? NullDisplayText => throw new NotImplementedException();

        public override IPropertyFilterProvider? PropertyFilterProvider => throw new NotImplementedException();

        public override bool ShowForDisplay => throw new NotImplementedException();

        public override bool ShowForEdit => throw new NotImplementedException();

        public override string? SimpleDisplayProperty => throw new NotImplementedException();

        public override string? TemplateHint => throw new NotImplementedException();

        public override bool ValidateChildren => throw new NotImplementedException();

        public override IReadOnlyList<object> ValidatorMetadata => throw new NotImplementedException();

        public override Func<object, object?>? PropertyGetter => throw new NotImplementedException();

        public override Action<object, object?>? PropertySetter => throw new NotImplementedException();

        public FakeModelMetadata(ModelMetadataIdentity identity) : base(identity)
        { }

        public void MockIsEnum(bool isEnum)
        {
            _isEnum = isEnum;
        }

        public void MockIsReadOnly(bool isReadOnly)
        {
            _isReadOnly = isReadOnly;
        }

        public void MockBindingSource(BindingSource source)
        {
            _bindingSource = source;
        }
    }
}