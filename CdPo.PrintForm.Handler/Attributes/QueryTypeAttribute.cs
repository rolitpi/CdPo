using CdPo.Common.Enum;

namespace CdPo.PrintForm.Handler.Attributes;

/// <summary>
/// Атрибут определения типа механизма сбора данных
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class QueryTypeAttribute : Attribute
{
    /// <summary>
    /// Тип механизма сбора данных
    /// </summary>
    public PrintFileType PrintFormType { get; }

    public QueryTypeAttribute(PrintFileType printFormType) => PrintFormType = printFormType;
}