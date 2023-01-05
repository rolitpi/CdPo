using Castle.Windsor;

namespace CdPo.Common.Interfaces;

/// <summary>Интерфейс кастомной регистрации сервисов</summary>
public interface IWindsorServiceRegistration
{
    /// <summary>Регистрация сервисов в контейнере</summary>
    /// <param name="container">IWindsorContainer</param>
    void RegisterService(IWindsorContainer container);
}