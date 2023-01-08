using CdPo.Common.Attributes;

namespace CdPo.Common.Enum;

/// <summary>
/// Перечисление типов печатных форм
/// </summary>
public enum PrintFileType
{
    /// <summary>
    /// Ведомость ППП
    /// </summary>
    [Display("Ведомость ППП")]
    StatementPpp = 1,
    
    /// <summary>
    /// Журнал ППП
    /// </summary>
    [Display("Журнал ППП")]
    JournalPpp = 2,
    
    /// <summary>
    /// Заявление слушателя ППП
    /// </summary>
    [Display("Заявление слушателя ППП")]
    ListenerStatementPpp = 3,
    
    /// <summary>
    /// Личная карточка слушателя
    /// </summary>
    [Display("Личная карточка слушателя")]
    ListenerPersonalCard = 4,
    
    /// <summary>
    /// Отчет ППП
    /// </summary>
    [Display("Отчет ППП")]
    ReportPpp = 5,
    
    /// <summary>
    /// Приказ на зачисление ППП
    /// </summary>
    [Display("Приказ на зачисление ППП")]
    EnrollmentOrderPpp = 6,
    
    /// <summary>
    /// Приказ на отчисление и выдачу диплома
    /// </summary>
    [Display("Приказ на отчисление и выдачу диплома")]
    ExpulsionOrderPpp = 7,
    
    /// <summary>
    /// Приказ на отчисление по собственному желанию
    /// </summary>
    [Display("Приказ на отчисление по собственному желанию")]
    DeductionAtWill = 8,
    
    /// <summary>
    /// Приказ о допуске к ИА
    /// </summary>
    [Display("Приказ о допуске к ИА")]
    IaAdmissionOrder = 9,
    
    /// <summary>
    /// Приказ о составе ИАК
    /// </summary>
    [Display("Приказ о составе ИАК")]
    IakCompositionOrder = 10,
    
    /// <summary>
    /// Приказ об отчислении без выдачи дипломов
    /// </summary>
    [Display("Приказ об отчислении без выдачи дипломов")]
    ExpulsionWithoutDiplomasOrder = 11,
    
    /// <summary>
    /// Протокол предзащиты
    /// </summary>
    [Display("Протокол предзащиты")]
    PreProtectionProtocol = 12,
    
    /// <summary>
    /// Расписание ИА
    /// </summary>
    [Display("Расписание ИА")]
    IaSchedule = 13,
    
    /// <summary>
    /// Согласие на распространение ПД слушателям
    /// </summary>
    [Display("Согласие на распространение ПД слушателям")]
    ListenersDistributionConsent = 14,
}