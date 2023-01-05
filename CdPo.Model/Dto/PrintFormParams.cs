namespace CdPo.Model.Dto;

/// <summary>
/// Параметры для формирования печаток
/// </summary>
public class PrintFormParams
{
    /// <summary>
    /// Идентификатор сущности
    /// </summary>
    public long EntityId { get; set; }

    /// <summary>
    /// Тип печатной формы
    /// </summary>
    public int PrintFormType { get; set; }
    
    /// <summary>
    /// Другие параметры
    /// </summary>
    public Dictionary<string, object> OtherParams { get; set; }
}