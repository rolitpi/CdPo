using CdPo.PrintForm.Handler.Attributes;

namespace CdPo.PrintForm.Handler.Models.Dto.Parameters;

/// <summary>
/// ДТО для печатной формы ведомости ППП
/// </summary>
public class StatementPppParametersDto: BaseReportParametersDto
{
    /// <summary>
    /// Форма обучения
    /// </summary>
    [FastReportParameter]
    public string EducationalForm { get; set; }
    
    /// <summary>
    /// Академический год
    /// </summary>
    [FastReportParameter]
    public string AcademicYear { get; set; }
    
    /// <summary>
    /// Форма контроля
    /// </summary>
    [FastReportParameter]
    public string ControlForm { get; set; }
    
    /// <summary>
    /// Группа ППП
    /// </summary>
    [FastReportParameter]
    public string PppGroup { get; set; }
    
    /// <summary>
    /// Дисциплина
    /// </summary>
    [FastReportParameter]
    public string Discipline { get; set; }
    
    /// <summary>
    /// Фамилия, имя, отчество преподавателя
    /// </summary>
    [FastReportParameter]
    public string TeacherFio { get; set; }
    
    /// <summary>
    /// Дата проведения экзамена
    /// </summary>
    [FastReportParameter]
    public string ExamDate { get; set; }
    
    /// <summary>
    /// ФИО директора ЦД и ПО
    /// </summary>
    [FastReportParameter]
    public string HeadFio { get; set; }
}