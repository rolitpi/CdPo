using System.Reflection;

using CdPo.PrintForm.Handler.Attributes;
using CdPo.PrintForm.Handler.Models;

namespace CdPo.PrintForm.Handler.Extensions;

public static class BaseReportParameterExtensions
{
    /// <summary>
    /// Вытянуть параметры из Dto в справочник имя_параметра - значение_параметра
    /// </summary>
    /// <param name="parametersDto"></param>
    /// <returns></returns>
    public static IDictionary<string, object> GetParametersDictionary(this BaseReportParametersDto parametersDto)
    {
        var resultDictionary = new Dictionary<string, object>();
        if (parametersDto == default)
        {
            return resultDictionary;
        }

        var propertyArray = parametersDto.GetType().GetProperties();
        foreach (var property in propertyArray)
        {
            var attributes = property.GetCustomAttributes()
                .OfType<FastReportParameterAttribute>()
                .ToArray();
            if (!attributes.Any())
            {
                continue;
            }

            var key = attributes.First().Name;
            var value = parametersDto.GetType().GetProperty(property.Name).GetValue(parametersDto, null);
            resultDictionary.Add(key, value);
        }

        return resultDictionary;
    }
}