using CdPo.PrintForm.Handler.Attributes;

namespace CdPo.PrintForm.Handler.Models.Dto.Parameters;

/// <summary>
/// ДТО для печатной формы ведомости ППП
/// </summary>
public class StatementPppParametersDto: BaseReportParametersDto
{
    /// <summary>
    /// Тестовая строка todo
    /// </summary>
    [FastReportParameter]
    public string TestStr1 { get; set; }
    
    /// <summary>
    /// Тестовая строка
    /// </summary>
    [FastReportParameter]
    public string TestStr2 { get; set; }
    
    /// <summary>
    /// Тестовая строка
    /// </summary>
    [FastReportParameter]
    public string TestStr3 { get; set; }
}