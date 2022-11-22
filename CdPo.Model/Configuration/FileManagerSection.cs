namespace CdPo.Model.Configuration;

/// <summary>
/// Конфигурация файлового менеджера.
/// </summary>
public class FileManagerSection
{
    /// <summary>
    /// Путь к папке кэша.
    /// </summary>
    public string Path { get; set; }
    
    /// <summary>
    /// Использовать FTP-сервер.
    /// </summary>
    public bool UseFtpServer { get; set; }
}