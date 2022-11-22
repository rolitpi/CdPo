namespace CdPo.Model.Interfaces.Files;

/// <summary>Интерфейс метаданных файла.</summary>
public interface IFileMetadata
{
    /// <summary>Идентификатор.</summary>
    long Id { get; set; }

    /// <summary>Наименование файла без расширения.</summary>
    string Name { get; set; }

    /// <summary>Расширение файла.</summary>
    string Extension { get; set; }

    /// <summary>Версия объекта.</summary>
    int ObjectVersion { get; set; }

    /// <summary>Дата создания объекта.</summary>
    DateTime ObjectCreateDate { get; set; }

    /// <summary>Дата изменения объекта.</summary>
    DateTime ObjectEditDate { get; set; }

    /// <summary>
    ///     Полное имя файла - наименование + '.' + расширение.
    /// </summary>
    string FullName { get; }

    /// <summary>
    ///     Размер файла в байтах.
    /// </summary>
    long Size { get; set; }
}