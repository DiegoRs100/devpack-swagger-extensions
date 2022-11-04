using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Devpack.Swagger.Extensions.Conventions
{
    public class ControllersHideConvension : IActionModelConvention
    {
        public void Apply(ActionModel action)
        {
            action.ApiExplorer.IsVisible = false;
        }
    }
}