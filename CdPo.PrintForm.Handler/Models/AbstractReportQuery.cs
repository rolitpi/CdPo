using System.Data;

using CdPo.Model.Interfaces;

namespace CdPo.PrintForm.Handler.Models;

/// <summary>
/// Абстрактный класс механизма сбора данных для отчёта
/// </summary>
public abstract class AbstractReportQuery
{
    /// <summary>
    /// Путь до директории с шаблонами
    /// </summary>
    public const string ReportRootPath = "CdPo.PrintForm.Handler.Content";
    
    /// <summary>
    /// Дополнительные параметры для формирования печатной формы
    /// </summary>
    public Dictionary<string, object> AdditionalParams { get; set; }
    
    /// <summary>
    /// Дефолтное имя датасета.
    /// Все табличные источники в шаблонах должны указывать значение этого параметра
    /// </summary>
    private const string DataSetDefaultName = "Data";

    /// <summary>
    /// Получить путь к шаблону отчета
    /// </summary>
    /// <returns></returns>
    public abstract string GetTemplatePath();
    
    /// <summary>
    /// Получить параметры отчёта
    /// </summary>
    /// <param name="database">Подключение к БД</param>
    /// <param name="entityId">Идентификатор сущности</param>
    /// <returns>DTO с параметрами для отчета</returns>
    public abstract Task<BaseReportParametersDto> GetReportParameters(IDataStore database, long entityId);

    /// <summary>
    /// Получить табличные источники отчёта
    /// </summary>
    /// <param name="database">Подключение к БД</param>
    /// <param name="entityId">Идентификатор сущности</param>
    /// <returns>DataSet с таблицами, которые содержатся в отчете</returns>
    public abstract Task<DataSet> GetReportData(IDataStore database, long entityId);
    
    /// <summary>
    /// Получить DataSet по умолчанию
    /// </summary>
    /// <returns>Пустой DataSet</returns>
    protected async Task<DataSet> GetDefaultDataSetAsync() => await Task.FromResult(new DataSet(DataSetDefaultName));
}