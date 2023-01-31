using Devpack.Swagger.Extensions.Conventions;
using Devpack.Swagger.Extensions.Tests.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using Xunit;

namespace Devpack.Swagger.Extensions.Tests
{
    public class ControllersHideConvensionTests
    {
        [Fact(DisplayName = "Deve setar o a propriedade (IsVisible) como false quando o método for chamado.")]
        public void Apply()
        {
            // Arrange
            var methodInfo = typeof(ObjectTest).GetMethod(nameof(ObjectTest.Method1));

            var actionModel = new ActionModel(methodInfo!, Array.Empty<object>())
            { 
                ApiExplorer = new ApiExplorerModel() { IsVisible = true },
            };

            var conversion = new ControllersHideConvension();

            // Act
            conversion.Apply(actionModel);

            // Asserts
            actionModel.ApiExplorer.IsVisible.Should().BeFalse();
        }
    }
}