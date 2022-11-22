using System.Reflection;

using CdPo.Model.Attributes;
using CdPo.Web.Controllers;

using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace CdPo.Web.Providers;

/// <inheritdoc />
public class GenericTypeControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
{
    /// <inheritdoc />
    public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
    {
        var currentAssembly = typeof(GeneratedControllerAttribute).Assembly;
        var candidates = currentAssembly.GetExportedTypes()
            .Where(x => x.GetCustomAttributes<GeneratedControllerAttribute>().Any());
            
        foreach (var candidate in candidates)
        {
            feature.Controllers.Add(
                typeof(BaseController<>).MakeGenericType(candidate).GetTypeInfo()
            );
        }
    }
}