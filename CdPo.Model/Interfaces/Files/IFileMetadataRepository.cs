namespace CdPo.Model.Interfaces.Files;

/// <summary>
/// Интерфейс провайдера метаданных файла.
/// </summary>
public interface IFileMetadataRepository
{
    /// <summary>
    /// Создать метаданные файла (с сохранением в БД).
    /// </summary>
    /// <param name="fileName">Имя файла без расширения.</param>
    /// <param name="extension">Расширение файла.</param>
    /// <param name="size">Размер файла.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Метаданные файла.</returns>
    Task<IFileMetadata> CreateFileMetadataAsync(string fileName, string extension, long size,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить метаданные файла по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор записи.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Метаданные файла.</returns>
    Task<IFileMetadata> GetFileMetadataAsync(long id, CancellationToken cancellationToken = default);
}