namespace CdPo.Common.Enum;

/// <summary>
/// Перечисление типов печатных форм
/// </summary>
public enum PrintFileType
{
    /// <summary>
    /// Ведомость ППП
    /// </summary>
    StatementPpp = 1,
    
    /// <summary>
    /// Журнал ППП
    /// </summary>
    JournalPpp = 2,
    
    /// <summary>
    /// Заявление слушателя ППП
    /// </summary>
    ListenerStatementPpp = 3,
    
    /// <summary>
    /// Личная карточка слушателя
    /// </summary>
    ListenerPersonalCard = 4,
    
    /// <summary>
    /// Отчет ППП
    /// </summary>
    ReportPpp = 5,
    
    /// <summary>
    /// Приказ на зачисление ППП
    /// </summary>
    EnrollmentOrderPpp = 6,
    
    /// <summary>
    /// Приказ на отчисление и выдачу диплома
    /// </summary>
    ExpulsionOrderPpp = 7,
    
    /// <summary>
    /// Приказ на отчисление по собственному желанию
    /// </summary>
    DeductionAtWill = 8,
    
    /// <summary>
    /// Приказ о допуске к ИА
    /// </summary>
    IaAdmissionOrder = 9,
    
    /// <summary>
    /// Приказ о составе ИАК
    /// </summary>
    IakCompositionOrder = 10,
    
    /// <summary>
    /// Приказ об отчислении без выдачи дипломов
    /// </summary>
    ExpulsionWithoutDiplomasOrder = 11,
    
    /// <summary>
    /// Протокол предзащиты
    /// </summary>
    PreProtectionProtocol = 12,
    
    /// <summary>
    /// Расписание ИА
    /// </summary>
    IaSchedule = 13,
    
    /// <summary>
    /// Согласие на распространение ПД слушателям
    /// </summary>
    ListenersDistributionConsent = 14,
}