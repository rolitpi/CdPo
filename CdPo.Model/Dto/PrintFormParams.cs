using CdPo.Common.Enum;

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
    /// Формат файла-результата
    /// </summary>
    public int PrintFileTypeInt { get; set; } = (int)PrintFileExt.Pdf;

    /// <summary>
    /// Другие параметры
    /// </summary>
    public Dictionary<string, object> OtherParams { get; set; } = new();
}