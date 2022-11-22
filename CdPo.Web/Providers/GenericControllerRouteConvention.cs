using System.Reflection;

using CdPo.Model.Attributes;
using CdPo.Web.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace CdPo.Web.Providers;

/// <inheritdoc />
public class GenericControllerRouteConvention : IControllerModelConvention
{
    /// <inheritdoc />
    public void Apply(ControllerModel controller)
    {
        if (!controller.ControllerType.IsGenericType || controller.ControllerType == typeof(BaseController))
        {
            return;
        }
        
        var genericType = controller.ControllerType.GenericTypeArguments[0];
        var customNameAttribute = genericType.GetCustomAttribute<GeneratedControllerAttribute>();

        if (customNameAttribute?.Route == null) return;
        
        controller.Selectors.Add(new SelectorModel
        {
            AttributeRouteModel = new AttributeRouteModel(new RouteAttribute($"api/{customNameAttribute.Route}")),
        });
        controller.ControllerName = customNameAttribute.FriendlyName;
    }
}