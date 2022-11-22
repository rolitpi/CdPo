using CdPo.Common.Enum;
using CdPo.Model.Configuration;
using CdPo.Model.Interfaces.Files;

using Microsoft.Extensions.Options;

namespace CdPo.Web.Services.Files;

/// <summary>
/// Базовый класс для поставщика файлов.
/// </summary>
public abstract class FileProvider: IFileProvider
{
    /// <summary>
    /// Путь к папке кэша.
    /// </summary>
    protected string CachePath { get; }
    
    /// <summary>
    /// Базовый класс для поставщика файлов.
    /// </summary>
    /// <param name="config">Конфигурация.</param>
    protected FileProvider(IOptions<FileManagerSection> config) => CachePath = config.Value.Path;
    
    /// <inheritdoc />
    public abstract Task<Stream> GetFileAsync(string path, CancellationToken cancellationToken = default);

    /// <inheritdoc />
    public virtual async Task<string> SaveFileAsync(byte[] data, CancellationToken cancellationToken = default)
    {
        return await SaveFileAsync(null, data, FileContentSourceType.ByteArray, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public virtual async Task<string> SaveFileAsync(Stream fileStream,
        CancellationToken cancellationToken = default)
    {
        return await SaveFileAsync(fileStream, null, FileContentSourceType.Stream, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public abstract Task<bool> CheckFileAsync(string path, CancellationToken cancellationToken = default);

    /// <inheritdoc />
    public abstract Task DeleteFileAsync(string path, CancellationToken cancellationToken = default);
    
    /// <summary>
    ///     Сохранить файл.
    /// </summary>
    /// <param name="fileStream">Поток с содержимым файла.</param>
    /// <param name="sourceType">Тип источника данных файла.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <param name="data">Содержимое файла.</param>
    /// <returns>Кэшированное имя файла.</returns>
    protected abstract Task<string> SaveFileAsync(Stream fileStream, byte[] data, FileContentSourceType sourceType,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    ///     Обработать содержимое файла в зависимости от типа входных данных.
    /// </summary>
    /// <param name="outStream">Выходной поток файла.</param>
    /// <param name="fileStream">Поток с содержимым файла.</param>
    /// <param name="data">Содержимое файла.</param>
    /// <param name="sourceType">Тип источника данных файла.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    protected virtual async Task ProcessFileContentAsync(Stream outStream, Stream fileStream, byte[] data,
        FileContentSourceType sourceType, CancellationToken cancellationToken = default)
    {
        switch (sourceType)
        {
            case FileContentSourceType.ByteArray:
            {
                if (data == null || data.Length == 0)
                    throw new ArgumentException("Отсутствуют данные файла");

                await SaveBytesAsync(outStream, data, cancellationToken).ConfigureAwait(false);
                break;
            }
            case FileContentSourceType.Stream:
            {
                if (fileStream == null || fileStream.Length == 0)
                    throw new ArgumentException("Отсутствуют данные файла");

                await SaveStreamAsync(outStream, fileStream, cancellationToken).ConfigureAwait(false);
                break;
            }
            default:
                throw new NotSupportedException("Неизвестный тип источника данных файла");
        }
    }
    
    /// <summary>
    ///     Сохранить содержимое файла в выходной поток.
    /// </summary>
    /// <param name="outStream">Поток файла.</param>
    /// <param name="data">Содержимое файла.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    protected virtual async Task SaveBytesAsync(Stream outStream, byte[] data,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await outStream.WriteAsync(data, 0, data.Length, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    ///     Сохранить поток в выходной поток файла.
    /// </summary>
    /// <param name="outStream">Поток файла.</param>
    /// <param name="inputStream">Входной поток.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    protected virtual async Task SaveStreamAsync(Stream outStream, Stream inputStream,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await inputStream.CopyToAsync(outStream, 81920, cancellationToken).ConfigureAwait(false);
    }
}