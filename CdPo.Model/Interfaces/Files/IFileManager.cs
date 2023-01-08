namespace CdPo.Model.Interfaces.Files;

/// <summary>Менеджер файлов</summary>
public interface IFileManager
{
    /// <summary>Системный путь к файлам</summary>
    DirectoryInfo SystemFilesPath { get; }

    /// <summary>Сохранить файл.</summary>
    /// <param name="fileStream">Поток.</param>
    /// <param name="fileName">Имя файла.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Метаданные файла.</returns>
    Task<IFileMetadata> SaveFileAsync(
        Stream fileStream,
        string fileName,
        CancellationToken cancellationToken = default);
    
    /// <summary>Получить файл (поток).</summary>
    /// <param name="id">Идентификатор метаданных файла.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Поток.</returns>
    Task<Stream> GetFileAsync(long id, CancellationToken cancellationToken = default);
    
    /// <summary>Получить файл (поток).</summary>
    /// <param name="file">Объект метаданных файла.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Поток.</returns>
    Task<Stream> GetFileAsync(IFileMetadata file, CancellationToken cancellationToken = default);
}