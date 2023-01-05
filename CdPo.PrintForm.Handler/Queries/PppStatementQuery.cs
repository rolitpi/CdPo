using System.Data;

using CdPo.Common.Enum;
using CdPo.Model.Interfaces;
using CdPo.PrintForm.Handler.Attributes;
using CdPo.PrintForm.Handler.Models;
using CdPo.PrintForm.Handler.Models.Dto.Parameters;

namespace CdPo.PrintForm.Handler.Queries;

[QueryType(PrintFileType.StatementPpp)]
public class PppStatementQuery: AbstractReportQuery
{
    public override string GetTemplatePath() => $"{ReportRootPath}.StatementPpp.frx";

    public override async Task<BaseReportParametersDto> GetReportParameters(IDataStore database, long entityId)
    {
        return await Task.FromResult(new StatementPppParametersDto
        {
            TestStr1 = "Тестовая строка первая",
            TestStr2 = "Тестовая строка вторая",
            TestStr3 = "Тестовая строка третья",
        });//todo
    }

    public override Task<DataSet> GetReportData(IDataStore database, long entityId)
        => GetDefaultDataSetAsync();
}