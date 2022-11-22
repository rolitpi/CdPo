namespace CdPo.Model.Interfaces.Files;

/// <summary>
/// Интерфейс поставщика файлов.
/// </summary>
public interface IFileProvider
{
    /// <summary>
    /// Получить файл.
    /// </summary>
    /// <param name="path">Путь к файлу.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Поток с содержимым файла.</returns>
    Task<Stream> GetFileAsync(string path, CancellationToken cancellationToken = default);

    /// <summary>
    /// Сохранить файл.
    /// </summary>
    /// <param name="data">Содержимое файла.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Кэшированное имя файла.</returns>
    Task<string> SaveFileAsync(byte[] data, CancellationToken cancellationToken = default);

    /// <summary>
    /// Сохранить файл.
    /// </summary>
    /// <param name="fileStream">Поток с содержимым файла.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Кэшированное имя файла.</returns>
    Task<string> SaveFileAsync(Stream fileStream, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Проверить наличие файла.
    /// </summary>
    /// <param name="path">Путь к файлу.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>True, если файл существует, иначе false.</returns>
    Task<bool> CheckFileAsync(string path, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Удалить файл.
    /// </summary>
    /// <param name="path">Путь к файлу.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    Task DeleteFileAsync(string path, CancellationToken cancellationToken = default);
}