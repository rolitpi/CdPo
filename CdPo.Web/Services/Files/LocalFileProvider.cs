using CdPo.Common.Enum;
using CdPo.Model.Configuration;

using Microsoft.Extensions.Options;

namespace CdPo.Web.Services.Files;

/// <summary>
/// Поставщик локальных файлов.
/// </summary>
public class LocalFileProvider : FileProvider
{
    /// <summary>
    /// Поставщик локальных файлов.
    /// </summary>
    /// <param name="config">Конфигурация.</param>
    public LocalFileProvider(IOptions<FileManagerSection> config) : base(config) {}
    
    /// <inheritdoc />
    public override async Task<Stream> GetFileAsync(string path, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var fullPath = GetFullPath(path);

        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException(fullPath);
        }

        return await Task.FromResult(new FileStream(fullPath, FileMode.Open, FileAccess.Read));
    }
    
    /// <summary>
    ///     Сохранить файл.
    /// </summary>
    /// <param name="fileStream">Поток с содержимым файла.</param>
    /// <param name="sourceType">Тип источника данных файла.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <param name="data">Содержимое файла.</param>
    /// <returns>Кэшированное имя файла.</returns>
    protected override async Task<string> SaveFileAsync(Stream fileStream, byte[] data,
        FileContentSourceType sourceType, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var currentDateDirectory = $"{DateTime.UtcNow:yyyy-MM}";
        var cachedName = $"{Path.GetRandomFileName()}";
        var path = Path.Combine(CachePath, currentDateDirectory, cachedName);

        EnsureDirectoryCreated(Path.GetFullPath(Path.Combine(CachePath, currentDateDirectory)));

        await using (var outStream = new FileStream(Path.GetFullPath(path), FileMode.CreateNew, FileAccess.Write))
        {
            await ProcessFileContentAsync(outStream, fileStream, data, sourceType, cancellationToken);
            await outStream.FlushAsync(cancellationToken);
            outStream.Close();
        }

        return Path.Combine(currentDateDirectory, cachedName);
    }

    /// <inheritdoc />
    public override async Task<bool> CheckFileAsync(string path, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await Task.FromResult(File.Exists(GetFullPath(path)));
    }
    
    /// <inheritdoc />
    public override async Task DeleteFileAsync(string path, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!await CheckFileAsync(path, cancellationToken))
        {
            throw new FileNotFoundException();
        }

        File.Delete(GetFullPath(path));
    }
    
    /// <summary>
    ///     Убедиться, что папка создана на диске.
    /// </summary>
    /// <param name="directory">Папка.</param>
    private static void EnsureDirectoryCreated(string directory)
    {
        var directoryInfo = new DirectoryInfo(directory);

        if (!directoryInfo.Exists)
        {
            directoryInfo.Create();
        }
    }

    /// <summary>
    ///     Получить полный путь к файлу.
    /// </summary>
    private string GetFullPath(string path)
    {
        return Path.GetFullPath(Path.Combine(
            string.Join(Path.DirectorySeparatorChar.ToString(), CachePath.Split('/', '\\')),
            string.Join(Path.DirectorySeparatorChar.ToString(), path.Split('/', '\\'))));
    }
}