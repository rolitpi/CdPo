namespace CdPo.Model.Extensions;

using Microsoft.Extensions.Configuration;

using System.IO;

/// <summary>
/// Расширения для <see cref="IConfiguration" />
/// </summary>
public static class IConfigurationExtensions
{
    /// <summary>
    /// Получить системный путь
    /// </summary>
    /// <param name="configuration">Конфигурация</param>
    public static DirectoryInfo GetSystemFilePath(this IConfiguration configuration)
    {
        var path = configuration.GetSection("AppSettings")
            .GetSection("SystemFilesPath")
            .Value;

        if (string.IsNullOrWhiteSpace(path))
        {
            return new DirectoryInfo(Path.GetTempPath());
        }

        var correct = path.Replace('/', Path.DirectorySeparatorChar)
            .Replace('\\', Path.DirectorySeparatorChar);

        return new DirectoryInfo(correct);
    }
}