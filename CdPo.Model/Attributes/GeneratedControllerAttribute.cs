namespace CdPo.Model.Attributes;

/// <summary>
/// Атрибут, для сущностей бд, который должен создавать контроллер
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class GeneratedControllerAttribute : Attribute
{
    public GeneratedControllerAttribute(string route = default, string friendlyName = default)
    {
        Route = route;
        FriendlyName = friendlyName;
    }
    
    /// <summary>
    /// Роут создающегося контроллера
    /// </summary>
    public string Route { get; }
    
    /// <summary>
    /// Человекочитаемое название контроллера
    /// </summary>
    public string FriendlyName { get; }
}