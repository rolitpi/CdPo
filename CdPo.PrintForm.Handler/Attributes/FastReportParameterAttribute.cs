using System.Runtime.CompilerServices;

namespace CdPo.PrintForm.Handler.Attributes;

/// <summary>
/// Параметр из отчёта
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
public class FastReportParameterAttribute : Attribute
{
    /// <summary>
    /// Наименование параметра
    /// </summary>
    public string Name { get; set; }

    public FastReportParameterAttribute([CallerMemberName] string name = default) => Name = name;
}