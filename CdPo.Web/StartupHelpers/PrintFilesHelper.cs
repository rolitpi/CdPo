using System.Reflection;

using Castle.Windsor;

using CdPo.Common.Enum;
using CdPo.Common.Extensions;
using CdPo.PrintForm.Handler.Attributes;
using CdPo.PrintForm.Handler.Models;

namespace CdPo.Web.StartupHelpers;

/// <summary>
/// Класс-помощник для регистрации механизмов сбора печатной формы
/// </summary>
public static class PrintFilesHelper
{
    /// <summary>
    /// Зарегистрировать механизмы для сборки печатной формы
    /// </summary>
    /// <param name="container"></param>
    public static void RegisterReportQueries(IWindsorContainer container)
    {
        var abstractReportQuerySubclassTypes = Assembly
            .GetAssembly(typeof(AbstractReportQuery))
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(AbstractReportQuery)) && !t.IsAbstract);

        foreach (var subclassType in abstractReportQuerySubclassTypes)
        {
            PrintFileType printFormType;

            printFormType = (subclassType.GetCustomAttributes()
                .FirstOrDefault(x => x is QueryTypeAttribute) as QueryTypeAttribute).PrintFormType;

            container.RegisterTransient(typeof(AbstractReportQuery), subclassType, 
                printFormType.ToString("D"));
        }
    }
}