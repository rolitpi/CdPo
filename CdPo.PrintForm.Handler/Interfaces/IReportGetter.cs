using System.Data;

namespace CdPo.PrintForm.Handler.Interfaces;

public interface IReportGetter : IDisposable
{
    /// <summary>
    /// Заполняет локальные свойства отчета
    /// </summary>
    /// <param name="properties">Словарь свойств и соответствующих им значений</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task StoreLocalPropertiesAsync(IDictionary<string, object> properties, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Возвращает отчет в виде потока
    /// </summary>
    /// <param name="template">Шаблон отчета</param>
    /// <param name="model">Модель данных</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task<Stream> GetReportAsync(Stream template, DataSet model, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Кол-во страниц в готовом файле отчета (с учетом данных в параметрах)
    /// </summary>
    public int? ReportPageCount { get; }
}