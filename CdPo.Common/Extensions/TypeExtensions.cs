using System.Collections;
using System.Reflection;
using System.Text;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace CdPo.Common.Extensions;

/// <summary>Набор методов-расширений для взаимодействия с типами.</summary>
  public static class TypeExtensions
  {
    private static IMemoryCache Cache = (IMemoryCache) new MemoryCache((IOptions<MemoryCacheOptions>) new MemoryCacheOptions());
    /// <summary>
    /// Невалидные символы для составных имен (имен видa Part1.Part2.Part3).
    /// </summary>
    public const string InvalidMultiPartNameChars = " +-*/^[]{}!\"\\%&()=?";
    /// <summary>
    /// Невалидные символы для имен членов (свойств, методов, классов и т.п.).
    /// </summary>
    public const string InvalidMemberNameChars = ". +-*/^[]{}!\"\\%&()=?";
    private static readonly Dictionary<Type, string> TypeAliases = new Dictionary<Type, string>()
    {
      {
        typeof (byte),
        "byte"
      },
      {
        typeof (sbyte),
        "sbyte"
      },
      {
        typeof (short),
        "short"
      },
      {
        typeof (ushort),
        "ushort"
      },
      {
        typeof (int),
        "int"
      },
      {
        typeof (uint),
        "uint"
      },
      {
        typeof (long),
        "long"
      },
      {
        typeof (ulong),
        "ulong"
      },
      {
        typeof (float),
        "float"
      },
      {
        typeof (double),
        "double"
      },
      {
        typeof (Decimal),
        "decimal"
      },
      {
        typeof (object),
        "object"
      },
      {
        typeof (bool),
        "bool"
      },
      {
        typeof (char),
        "char"
      },
      {
        typeof (string),
        "string"
      },
      {
        typeof (void),
        "void"
      },
      {
        typeof (byte?),
        "byte?"
      },
      {
        typeof (sbyte?),
        "sbyte?"
      },
      {
        typeof (short?),
        "short?"
      },
      {
        typeof (ushort?),
        "ushort?"
      },
      {
        typeof (int?),
        "int?"
      },
      {
        typeof (uint?),
        "uint?"
      },
      {
        typeof (long?),
        "long?"
      },
      {
        typeof (ulong?),
        "ulong?"
      },
      {
        typeof (float?),
        "float?"
      },
      {
        typeof (double?),
        "double?"
      },
      {
        typeof (Decimal?),
        "decimal?"
      },
      {
        typeof (bool?),
        "bool?"
      },
      {
        typeof (char?),
        "char?"
      }
    };
    private static readonly Dictionary<string, string> BackwardCompatibilityReplacements = new Dictionary<string, string>()
    {
      {
        "BarsUp",
        "Bars.B4"
      },
      {
        "System.Private.CoreLib",
        "mscorlib"
      },
      {
        "System.Runtime",
        "mscorlib"
      }
    };

    /// <summary>
    /// Получение списка свойств типа по указанным флагам.
    /// По умолчанию возвращает public-свойства уровня экземпляра.
    /// Реализован простым вызовом Type.GetProperties.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="flags"></param>
    /// <returns></returns>
    public static IEnumerable<PropertyInfo> Properties(
      this Type type,
      BindingFlags flags = BindingFlags.Instance | BindingFlags.Public)
    {
      return (IEnumerable<PropertyInfo>) type.GetProperties(flags);
    }
    
    /// <summary>
    /// Проверяет является ли тип приводимым к другому типу.
    /// Маркер checkInheritance нерабочий, цепочка наследования проверяется в любом случае.
    /// В текущем состоянии метод возвращает true, если класс имеет в цепочке наследования класс baseType
    /// или интерфейс имеет в цепочке наследования интерфейс baseType
    /// или класс реализует или наследует реализацию интерфейса baseType.
    /// </summary>
    /// <param name="type">Проверяемый тип.</param>
    /// <typeparam name="TType">Предполагаемый базовый тип.</typeparam>
    /// <returns></returns>
    public static bool Is<TType>(this Type type)
    {
      Type baseType = typeof (TType);
      return TypeExtensions.Is(type, baseType);
    }
    
    /// <summary>
    /// Проверяет является ли тип приводимым к другому типу.
    /// Маркер checkInheritance нерабочий, цепочка наследования проверяется в любом случае.
    /// В текущем состоянии метод возвращает true, если класс имеет в цепочке наследования класс baseType
    /// или интерфейс имеет в цепочке наследования интерфейс baseType
    /// или класс реализует или наследует реализацию интерфейса baseType.
    /// </summary>
    /// <param name="type">Проверяемый тип.</param>
    /// <param name="baseType">Предполагаемый базовый тип.</param>
    /// <returns></returns>
    public static bool Is(this Type type, Type baseType)
    {
      return baseType.IsAssignableFrom(type);
    }

    /// <summary>
    /// Возвращает уровень типа в цепочке наследования.
    /// Для null вернет Int32.MaxValue.
    /// Для интерфейсов всегда возвращает 0.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static int InheritanceLevel(this Type type) => type == (Type) null ? int.MaxValue : (type.BaseType == (Type) null ? 0 : type.BaseType.InheritanceLevel() + 1);
  }