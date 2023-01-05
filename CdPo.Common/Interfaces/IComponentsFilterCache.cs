namespace CdPo.Common.Interfaces;

/// <summary>
/// Вспомогательный интерфейс для фильтрации и селекции обработчиков контейнера.
/// Позволяет задавать и получать игнорируемые имплементации сервиса.
/// </summary>
public interface IComponentsFilterCache
{
    /// <summary>
    /// Добавляет имплементацию сервиса в список игнорируемых.
    /// </summary>
    /// <param name="service"></param>
    /// <param name="implementation"></param>
    void Add(Type service, Type implementation);

    /// <summary>
    /// Добавляет имплементацию сервиса в список игнорируемых.
    /// </summary>
    /// <param name="implementation"></param>
    /// <typeparam name="TService"></typeparam>
    void Add<TService>(Type implementation);

    /// <summary>Возвращает список игнорируемых имплементаций сервиса.</summary>
    /// <param name="service"></param>
    /// <returns></returns>
    IList<Type> GetIgnorList(Type service);
}