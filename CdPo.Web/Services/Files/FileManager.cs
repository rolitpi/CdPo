using CdPo.Model.Configuration;
using CdPo.Model.Extensions;
using CdPo.Model.Interfaces.Files;

using Microsoft.Extensions.Options;

namespace CdPo.Web.Services.Files;

/// <inheritdoc/>
public class FileManager: IFileManager
{
    /// <summary>
    /// Репозиторий файловых метаданных
    /// </summary>
    protected readonly IFileMetadataRepository FileMetadataRepository;

    /// <summary>
    /// Провайдер физических файлов.
    /// </summary>
    protected readonly IFileProvider FileProvider;

    /// <summary>
    /// Путь к системной папке
    /// </summary>
    protected readonly DirectoryInfo InternalSystemFilesPath;

    protected readonly ILogger<FileManager> Logger;
    
    /// <inheritdoc/>
    public DirectoryInfo SystemFilesPath => InternalSystemFilesPath;

    public FileManager(IFileProvider fileProvider,
        IConfiguration configuration,
        IFileMetadataRepository fileMetadataRepository,
        IOptions<FileManagerSection> fileManagerConfig,
        ILogger<FileManager> logger)
    {
        var systemFilesPath = configuration.GetSystemFilePath();

        var config = fileManagerConfig?.Value;
        ValidateConfiguration(config, systemFilesPath.FullName);

        if (!config.UseFtpServer)
        {
            systemFilesPath = new DirectoryInfo(config.Path);
        }

        InternalSystemFilesPath = systemFilesPath;
        FileProvider = fileProvider;
        FileMetadataRepository = fileMetadataRepository;
        Logger = logger;
    }

    /// <inheritdoc />
    public Task<Stream> GetFileAsync(long id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (id <= 0)
        {
            throw new ArgumentException("Указан некорректный идентификатор метаданных файла!");
        }

        return GetFileInternalAsync(id, cancellationToken);
    }

    /// <inheritdoc />
    public Task<Stream> GetFileAsync(IFileMetadata file, CancellationToken cancellationToken = default)
    {
        if (file == default)
        {
            throw new ArgumentException("Передан пустой объект метаданных файла");
        }

        return GetFileAsync(file.Id, cancellationToken);
    }

    /// <summary>
    ///     Разделить наименование и расширение файла.
    /// </summary>
    /// <param name="fileNameWithExtension">Имя файла с расширением.</param>
    /// <returns>Наименование и расширение файла.</returns>
    protected static (string fileName, string fileExtension) SplitNameAndExtension(string fileNameWithExtension)
    {
        var indexOfDot = fileNameWithExtension.LastIndexOf('.');
        var fileExtension = fileNameWithExtension[(indexOfDot + 1)..];
        var fileName = fileNameWithExtension[..indexOfDot];
        return (fileName, fileExtension);
    }
    
    /// <inheritdoc />
    Task<IFileMetadata> IFileManager.SaveFileAsync(Stream fileStream, string fileName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (fileStream == null)
        {
            throw new ArgumentException("Не переданы данные для сохранения!");
        }

        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("Указано пустое наименование файла!");
        }

        return SaveFileInternalAsync(fileStream, fileName, cancellationToken);
    }
    
    private async Task<IFileMetadata> SaveFileInternalAsync(Stream content, string fileName, CancellationToken cancellationToken)
    {
        var (fileNameWithoutExtension, extension) = SplitNameAndExtension(fileName);
        var size = content.Length;
        var cachedName = await FileProvider.SaveFileAsync(content, cancellationToken);
        return await FileMetadataRepository.CreateFileMetadataAsync(fileNameWithoutExtension, extension, size, cachedName,
            cancellationToken);
    }
    
    private async Task<Stream> GetFileInternalAsync(long id, CancellationToken cancellationToken)
    {
        try
        {
            var metadata = await FileMetadataRepository.GetFileMetadataAsync(id, cancellationToken);
            return await FileProvider.GetFileAsync(metadata.CachedName, cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"Ошибка при получении контента файла [id={id}]");
            throw;
        }
    }
    
    private static void ValidateConfiguration(FileManagerSection configuration, string systemFilesPath)
    {
        if (configuration == null)
        {
            throw new InvalidOperationException("Не заполнена секция \"FileManager\" в конфигурации приложения.");
        }
    }
}