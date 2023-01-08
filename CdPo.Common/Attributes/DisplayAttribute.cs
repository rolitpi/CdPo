namespace CdPo.Common.Attributes;

/// <summary>
/// Атрибут, позволяющий указать отображаемое имя
/// </summary>
[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
public class DisplayAttribute: Attribute
{
    public string Name { get; set; }

    public DisplayAttribute(string name)
    {
        Name = name;
    }
    
    public DisplayAttribute(): this(string.Empty) {}
}