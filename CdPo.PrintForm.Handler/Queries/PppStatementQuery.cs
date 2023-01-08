using System.Data;

using CdPo.Common.Enum;
using CdPo.Model.Interfaces;
using CdPo.PrintForm.Handler.Attributes;
using CdPo.PrintForm.Handler.Extensions;
using CdPo.PrintForm.Handler.Models;
using CdPo.PrintForm.Handler.Models.Dto.Data;
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
            EducationalForm = "Очно – заочная форма обучения",
            ControlForm = "экзамен",
            PppGroup = "Практическая психология: диагностика и консультирование личности",
            Discipline = "Научные основы практической психологии (18 ч.)",
            TeacherFio = "Скоробогатько Сергей Петрович",
            AcademicYear = "2022-2023 учебный год",
            ExamDate = "12 апреля 2025",
            HeadFio = "Т.В. Кирилина"
        });//todo
    }

    public override async Task<DataSet> GetReportData(IDataStore database, long entityId)
    {
        var data = await GetDefaultDataSetAsync();
        var testData = new List<StatementPppExamineesData>
        {
            new() { FullName = "Аксёнова В.С." },
            new() { FullName = "Жидкова А.В." },
            new() { FullName = "Зыкова М.В." },
            new() { FullName = "Калугина Т.И." },
            new() { FullName = "Острянина В.А." },
            new() { FullName = "Постникова А.В." },
            new() { FullName = "Романова Е.В." },
            new() { FullName = "Савищева Д.А." },
            new() { FullName = "Суркова В.В." },
            new() { FullName = "Филина А.Ю." },
            new() { FullName = "Ямковая Е.В." },
            new() { FullName = "Аксёнова В.С." },
            new() { FullName = "Жидкова А.В." },
            new() { FullName = "Зыкова М.В." },
            new() { FullName = "Калугина Т.И." },
            new() { FullName = "Острянина В.А." },
            new() { FullName = "Постникова А.В." },
            new() { FullName = "Романова Е.В." },
            new() { FullName = "Савищева Д.А." },
            new() { FullName = "Суркова В.В." },
            new() { FullName = "Филина А.Ю." },
            new() { FullName = "Ямковая Е.В." },
        };//todo

        data.Tables.Add(testData.ToDataTable("Examinees"));
        return data;
    }
}