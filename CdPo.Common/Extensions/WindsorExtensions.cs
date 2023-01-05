using System.Collections;
using System.Reflection;

using Castle.MicroKernel;
using Castle.MicroKernel.ComponentActivator;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

using CdPo.Common.Interfaces;
using CdPo.Common.IoC;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CdPo.Common.Extensions;

/// <summary>Расширения для IoC-контейнера</summary>
  public static class WindsorExtensions
  {
    /// <summary>Добавить IFacility, если его нет</summary>
    /// <typeparam name="TImpl">Имплементация</typeparam>
    /// <param name="container">Контейнер</param>
    public static void EnsureFacilityIsAdded<TImpl>(this IWindsorContainer container) where TImpl : IFacility, new()
    {
      if (!((IEnumerable<IFacility>) container.Kernel.GetFacilities()).All<IFacility>((Func<IFacility, bool>) (x => x.GetType() != typeof (TImpl))))
        return;
      container.AddFacility<TImpl>();
    }

    /// <summary>Зарегистрировать имплементацию IWindsorServiceRegistration</summary>
    /// <typeparam name="TImpl">Имплементация</typeparam>
    /// <param name="container">Контейнер</param>
    /// <param name="name">Имя для регистрации</param>
    public static void RegisterWindsorServiceRegistration<TImpl>(
      this IWindsorContainer container,
      string name = null)
      where TImpl : IWindsorServiceRegistration
    {
      container.RegisterTransient<IWindsorServiceRegistration, TImpl>(name);
    }

    /// <summary>Регистрация компонента в контейнере</summary>
    /// <param name="registration"></param>
    /// <param name="container"></param>
    public static IWindsorContainer RegisterIn(
      this IRegistration registration,
      IWindsorContainer container)
    {
      if (registration == null)
        throw new ArgumentNullException(nameof (registration));
      if (container == null)
        throw new ArgumentNullException(nameof (container));
      container.Register(registration);
      return container;
    }

    /// <summary>Регистрация компонента в контейнере</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="registration"></param>
    /// <param name="container"></param>
    public static IWindsorContainer RegisterIn<T>(
      this ComponentRegistration<T> registration,
      IWindsorContainer container)
      where T : class
    {
      if (registration == null)
        throw new ArgumentNullException(nameof (registration));
      if (container == null)
        throw new ArgumentNullException(nameof (container));
      container.Register((IRegistration) registration);
      return container;
    }

    /// <summary>Установка инсталлера в контейнере</summary>
    /// <param name="installer"></param>
    /// <param name="container"></param>
    public static void InstallIn(this IWindsorInstaller installer, IWindsorContainer container)
    {
      if (installer == null)
        throw new ArgumentNullException(nameof (installer));
      if (container == null)
        throw new ArgumentNullException(nameof (container));
      container.Install(installer);
    }

    /// <summary>
    /// Возвращает истину, если указанный тип был зарегистрирован.
    /// </summary>
    /// <param name="container">Контейнер.</param>
    /// <typeparam name="T">Тип объекта.</typeparam>
    /// <returns></returns>
    public static bool HasComponent<T>(this IWindsorContainer container) => container.Kernel.HasComponent(typeof (T));

    /// <summary>
    /// Возвращает истину, если компонент с заданным именем был зарегистрирован.
    /// </summary>
    /// <param name="container">Контейнер.</param>
    /// <param name="key">Имя.</param>
    /// <returns></returns>
    public static bool HasComponent(this IWindsorContainer container, string key) => container.Kernel.HasComponent(key);

    /// <summary>
    /// Заполнение свойств объекта компонентами, зарегистрированными в контейнере
    /// </summary>
    /// <param name="container">Windsor-контейнер, из которого извлечь зависимости объекта</param>
    /// <param name="target">Объект, который подлежит заполнению</param>
    /// <exception cref="T:Castle.MicroKernel.ComponentActivator.ComponentActivatorException">
    /// </exception>
    public static IWindsorContainer BuildUp(
      this IWindsorContainer container,
      object target)
    {
      Type type = target.GetType();
      IKernel kernel = container.Kernel;
      foreach (PropertyInfo property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
      {
        if (property.CanWrite && kernel.HasComponent(property.PropertyType))
        {
          object obj = kernel.Resolve(property.PropertyType);
          try
          {
            property.SetValue(target, obj, (object[]) null);
          }
          catch (Exception ex)
          {
            throw new ComponentActivatorException(string.Format("Error setting property {0} on type {1}, See inner exception for more information.", (object) property.Name, (object) type.FullName), ex, kernel.GetHandler(property.PropertyType).ComponentModel);
          }
        }
      }
      return container;
    }

    /// <summary>Освобождение внедренных сервисов</summary>
    /// <param name="container"></param>
    /// <param name="objToConfigure"></param>
    public static void TearDown(this IWindsorContainer container, object objToConfigure)
    {
      foreach (PropertyInfo property in objToConfigure.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
      {
        object instance1 = property.GetValue(objToConfigure, (object[]) null);
        if (instance1 != null)
        {
          if (((IEnumerable<Type>) property.PropertyType.GetInterfaces()).Where<Type>((Func<Type, bool>) (@interface => @interface.IsGenericType)).Any<Type>((Func<Type, bool>) (@interface => @interface.GetGenericTypeDefinition() == typeof (ICollection<>))))
          {
            if (instance1 is ICollection collection)
            {
              foreach (object instance2 in (IEnumerable) collection)
                container.Release(instance2);
            }
          }
          else if (property.PropertyType.IsInterface || property.PropertyType.IsClass)
            container.Release(instance1);
        }
      }
    }

    /// <summary>
    /// Включение возможности использовать механизмов <see cref="T:BarsUp.IoC.IDependenciesResolutionBag" />
    /// для упрощенного инжекта множества параметров в конструкторы
    /// </summary>
    /// <param name="container"></param>
    /// <returns></returns>
    public static IWindsorContainer EnableDependencyResolutionBag(
      this IWindsorContainer container)
    {
      container.Kernel.Resolver.AddSubResolver((ISubDependencyResolver) new DependenciesResolutionBagResolver(container));
      return container;
    }

    /// <summary>Зарегистрировать имплементацию как SessionScoped</summary>
    /// <typeparam name="TService">Интерфейс</typeparam>
    /// <typeparam name="TImpl">Имплементация</typeparam>
    /// <param name="container">Контайнер</param>
    /// <param name="name">Имя для регистрации</param>
    public static void RegisterSessionScoped<TService, TImpl>(
      this IWindsorContainer container,
      string name = null)
      where TService : class
      where TImpl : TService
    {
      Component.For<TService>().ImplementedBy<TImpl>().Named(name).LifestyleScoped().RegisterIn<TService>(container);
    }

    /// <summary>Зарегистрировать имплементацию как SessionScoped</summary>
    /// <param name="container">Контайнер</param>
    /// <param name="serviceType">Тип интерфейса</param>
    /// <param name="implType">Тип имплементации</param>
    /// <param name="name">Имя для регистрации</param>
    public static void RegisterSessionScoped(
      this IWindsorContainer container,
      Type serviceType,
      Type implType,
      string name = null)
    {
      Component.For(serviceType).ImplementedBy(implType).Named(name).LifestyleScoped().RegisterIn<object>(container);
    }

    /// <summary>Зарегистрировать имплементацию как SessionScoped</summary>
    /// <typeparam name="TServiceOne">Интерфейс 1</typeparam>
    /// <typeparam name="TServiceTwo">Интерфейс 2</typeparam>
    /// <typeparam name="TImpl">Имплементация</typeparam>
    /// <param name="container">Контайнер</param>
    /// <param name="name">Имя для регистрации</param>
    public static void RegisterSessionScoped<TServiceOne, TServiceTwo, TImpl>(
      this IWindsorContainer container,
      string name = null)
      where TServiceOne : class
      where TServiceTwo : class
      where TImpl : TServiceOne, TServiceTwo
    {
      Component.For<TServiceOne, TServiceTwo>().ImplementedBy<TImpl>().Named(name).LifestyleScoped().RegisterIn<TServiceOne>(container);
    }

    /// <summary>Зарегистрировать имплементацию как SessionScoped</summary>
    /// <typeparam name="TServiceOne">Интерфейс 1</typeparam>
    /// <typeparam name="TServiceTwo">Интерфейс 2</typeparam>
    /// <typeparam name="TServiceThree">Интерфейс 3</typeparam>
    /// <typeparam name="TImpl">Имплементация</typeparam>
    /// <param name="container">Контайнер</param>
    /// <param name="name">Имя для регистрации</param>
    public static void RegisterSessionScoped<TServiceOne, TServiceTwo, TServiceThree, TImpl>(
      this IWindsorContainer container,
      string name = null)
      where TServiceOne : class
      where TServiceTwo : class
      where TServiceThree : class
      where TImpl : TServiceOne, TServiceTwo, TServiceThree
    {
      Component.For<TServiceOne, TServiceTwo, TServiceThree>().ImplementedBy<TImpl>().Named(name).LifestyleScoped().RegisterIn<TServiceOne>(container);
    }

    /// <summary>Зарегистрировать имплементацию как Transient</summary>
    /// <param name="container">Контайнер</param>
    /// <param name="serviceType">Тип интерфейса</param>
    /// <param name="implType">Тип имплементации</param>
    /// <param name="name">Имя для регистрации</param>
    public static void RegisterTransient(
      this IWindsorContainer container,
      Type serviceType,
      Type implType,
      string name = null)
    {
      Component.For(serviceType).ImplementedBy(implType).Named(name).LifestyleTransient().RegisterIn<object>(container);
    }

    /// <summary>Зарегистрировать имплементацию как Transient</summary>
    /// <typeparam name="TService">Интерфейс</typeparam>
    /// <typeparam name="TImpl">Имплементация</typeparam>
    /// <param name="container">Контайнер</param>
    /// <param name="name">Имя для регистрации</param>
    public static void RegisterTransient<TService, TImpl>(
      this IWindsorContainer container,
      string name = null)
      where TService : class
      where TImpl : TService
    {
      Component.For<TService>().ImplementedBy<TImpl>().Named(name).LifestyleTransient().RegisterIn<TService>(container);
    }

    /// <summary>Зарегистрировать имплементацию как Transient</summary>
    /// <typeparam name="TServiceOne">Интерфейс 1</typeparam>
    /// <typeparam name="TServiceTwo">Интерфейс 2</typeparam>
    /// <typeparam name="TImpl">Имплементация</typeparam>
    /// <param name="container">Контайнер</param>
    /// <param name="name">Имя для регистрации</param>
    public static void RegisterTransient<TServiceOne, TServiceTwo, TImpl>(
      this IWindsorContainer container,
      string name = null)
      where TServiceOne : class
      where TServiceTwo : class
      where TImpl : TServiceOne, TServiceTwo
    {
      Component.For<TServiceOne, TServiceTwo>().ImplementedBy<TImpl>().Named(name).LifestyleTransient().RegisterIn<TServiceOne>(container);
    }

    /// <summary>Зарегистрировать имплементацию как Transient</summary>
    /// <typeparam name="TServiceOne">Интерфейс 1</typeparam>
    /// <typeparam name="TServiceTwo">Интерфейс 2</typeparam>
    /// <typeparam name="TServiceThree">Интерфейс 3</typeparam>
    /// <typeparam name="TImpl">Имплементация</typeparam>
    /// <param name="container">Контайнер</param>
    /// <param name="name">Имя для регистрации</param>
    public static void RegisterTransient<TServiceOne, TServiceTwo, TServiceThree, TImpl>(
      this IWindsorContainer container,
      string name = null)
      where TServiceOne : class
      where TServiceTwo : class
      where TServiceThree : class
      where TImpl : TServiceOne, TServiceTwo, TServiceThree
    {
      Component.For<TServiceOne, TServiceTwo, TServiceThree>().ImplementedBy<TImpl>().Named(name).LifestyleTransient().RegisterIn<TServiceOne>(container);
    }

    /// <summary>Зарегистрировать имплементацию как Singleton</summary>
    /// <typeparam name="TService">Интерфейс</typeparam>
    /// <typeparam name="TImpl">Имплементация</typeparam>
    /// <param name="container">Контайнер</param>
    /// <param name="name">Имя для регистрации</param>
    public static void RegisterSingleton<TService, TImpl>(
      this IWindsorContainer container,
      string name = null)
      where TService : class
      where TImpl : TService
    {
      Component.For<TService>().ImplementedBy<TImpl>().Named(name).LifestyleSingleton().RegisterIn<TService>(container);
    }

    /// <summary>Зарегистрировать имплементацию как Singleton</summary>
    /// <typeparam name="TServiceOne">Интерфейс 1</typeparam>
    /// <typeparam name="TServiceTwo">Интерфейс 2</typeparam>
    /// <typeparam name="TImpl">Имплементация</typeparam>
    /// <param name="container">Контайнер</param>
    /// <param name="name">Имя для регистрации</param>
    public static void RegisterSingleton<TServiceOne, TServiceTwo, TImpl>(
      this IWindsorContainer container,
      string name = null)
      where TServiceOne : class
      where TServiceTwo : class
      where TImpl : TServiceOne, TServiceTwo
    {
      Component.For<TServiceOne, TServiceTwo>().ImplementedBy<TImpl>().Named(name).LifestyleSingleton().RegisterIn<TServiceOne>(container);
    }

    /// <summary>Зарегистрировать имплементацию как Singleton</summary>
    /// <param name="container"></param>
    /// <typeparam name="TImpl"></typeparam>
    public static void RegisterSingleton<TImpl>(this IWindsorContainer container) where TImpl : class => container.RegisterSingleton<TImpl, TImpl>();

    /// <summary>Замена компонента в контейнере.</summary>
    /// <param name="container">
    /// </param>
    /// <param name="serviceType">Тип сервиса.</param>
    /// <param name="implementationType">
    /// Тип имплементации, которую нужно заменить.
    /// </param>
    /// <param name="newComponent">Новый компонент.</param>
    public static IWindsorContainer ReplaceComponent(
      this IWindsorContainer container,
      Type serviceType,
      Type implementationType,
      IRegistration newComponent)
    {
      container.Register(newComponent);
      container.Resolve<IComponentsFilterCache>().Add(serviceType, implementationType);
      return container;
    }

    /// <summary>Замена компонента в контейнере.</summary>
    /// <param name="container">
    /// </param>
    /// <typeparam name="TService">Тип сервиса.</typeparam>
    /// <param name="implementationType">
    /// Тип имплементации, которую нужно заменить.
    /// </param>
    /// <param name="newComponent">Новый компонент.</param>
    public static IWindsorContainer ReplaceComponent<TService>(
      this IWindsorContainer container,
      Type implementationType,
      IRegistration newComponent)
    {
      return container.ReplaceComponent(typeof (TService), implementationType, newComponent);
    }

    /// <summary>Замена компонента в контейнере.</summary>
    /// <typeparam name="TService">Тип сервиса.</typeparam>
    /// <typeparam name="TImplementation">
    /// Тип имплементации, которую нужно заменить.
    /// </typeparam>
    /// <param name="container"></param>
    /// <param name="component">Новый компонент.</param>
    /// <returns></returns>
    public static IWindsorContainer ReplaceComponent<TService, TImplementation>(
      this IWindsorContainer container,
      IRegistration component)
    {
      return container.ReplaceComponent(typeof (TService), typeof (TImplementation), component);
    }

    /// <summary>
    /// Замена регистрации имплементатора сервиса.
    /// Применим, если ТОЛЬКО происходит замена единичной регистрации.
    /// При наличии нескольких регистраций сервиса произойдет ошибка.
    /// </summary>
    /// <typeparam name="TService">Тип сервиса.</typeparam>
    /// <typeparam name="TImplementor">Тип новой имплементации.</typeparam>
    /// <param name="container"></param>
    /// <param name="isTransient">
    /// Флаг указывающий на временный стиль жизни нового компонента.
    /// Если равен false, то создается Singleton-компонент.
    /// </param>
    /// <returns></returns>
    public static IWindsorContainer ReplaceImplementor<TService, TImplementor>(
      this IWindsorContainer container,
      bool isTransient = false)
      where TService : class
      where TImplementor : TService
    {
      ComponentRegistration<TService> componentRegistration = Component.For<TService>().ImplementedBy<TImplementor>();
      ComponentRegistration<TService> component = isTransient ? componentRegistration.LifestyleTransient() : componentRegistration.LifestyleSingleton();
      return container.ReplaceComponent<TService>(component);
    }

    /// <summary>
    /// Замена регистрации сервиса.
    /// Применим, если ТОЛЬКО происходит замена единичной регистрации.
    /// При наличии нескольких регистраций сервиса произойдет ошибка.
    /// </summary>
    /// <typeparam name="TService">Тип сервиса.</typeparam>
    /// <param name="container"></param>
    /// <param name="component">Новый компонент.</param>
    /// <returns></returns>
    public static IWindsorContainer ReplaceComponent<TService>(
      this IWindsorContainer container,
      ComponentRegistration<TService> component)
      where TService : class
    {
      IHandler handler = container.Kernel.GetHandler(typeof (TService));
      if (handler != null)
        container.Resolve<IComponentsFilterCache>().Add(typeof (TService), handler.ComponentModel.Implementation);
      component.RegisterIn<TService>(container);
      return container;
    }

    /// <summary>
    /// Разрешить все сервисы и вернуть обертку, реализующую <see cref="T:System.IDisposable" />,
    /// при освобождении которого будет освобождены сервисы (<see cref="M:Castle.Windsor.IWindsorContainer.Release(System.Object)" />)
    /// В случае если сервис реализует <see cref="T:System.IDisposable" />, он также будет освобожден.
    /// </summary>
    /// <param name="container"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static WindsorExtensions.ResolvedDisposables<T> ResolveAllAsDisposable<T>(
      this IWindsorContainer container,
      Arguments args = null)
    {
      return new WindsorExtensions.ResolvedDisposables<T>(container, typeof (T), args);
    }

    /// <summary>
    /// Разрешить все сервисы и вернуть обертку, реализующую <see cref="T:System.IDisposable" />,
    /// при освобождении которого будет освобождены сервисы (<see cref="M:Castle.Windsor.IWindsorContainer.Release(System.Object)" />)
    /// В случае если сервис реализует <see cref="T:System.IDisposable" />, он также будет освобожден.
    /// </summary>
    /// <param name="container"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static WindsorExtensions.ResolvedDisposables<T> ResolveAllAsDisposable<T>(
      this IWindsorContainer container,
      Type serviceType,
      Arguments args = null)
    {
      return new WindsorExtensions.ResolvedDisposables<T>(container, typeof (T), args);
    }

    /// <summary>
    /// Разрешить сервис и вернуть обертку, реализующую <see cref="T:System.IDisposable" />,
    /// при освобождении которого будет освобожден сервис (<see cref="M:Castle.Windsor.IWindsorContainer.Release(System.Object)" />)
    /// В случае если сервис реализует <see cref="T:System.IDisposable" />, он также будет освобожден.
    /// </summary>
    /// <param name="container"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static WindsorExtensions.ResolvedDisposable<T> ResolveAsDisposable<T>(
      this IWindsorContainer container,
      Arguments args = null)
    {
      return new WindsorExtensions.ResolvedDisposable<T>(container, args: args);
    }

    public static WindsorExtensions.ResolvedDisposable<T> ResolveAsDisposable<T>(
      this IWindsorContainer container,
      Type serviceType,
      Arguments args = null)
    {
      return new WindsorExtensions.ResolvedDisposable<T>(container, serviceType, args);
    }

    /// <summary>
    /// Метод расширение для обертки конструкции резрешения интерфейса, выполнения с реализацией действий и высвобождения ресурсов
    /// Использовать только для компонентов с жизненым циклом Transient
    /// </summary>
    /// <param name="container"></param>
    /// <param name="serviceType"></param>
    /// <param name="key"></param>
    /// <param name="arguments"></param>
    /// <param name="action"></param>
    public static void UsingForResolved(
      this IWindsorContainer container,
      Type serviceType,
      string key,
      object arguments,
      Action<IWindsorContainer, object> action)
    {
      object handler = container.Resolve(key, serviceType, Arguments.FromProperties(arguments));
      WindsorExtensions.HandlerActionInvoker<object>(container, handler, action);
    }

    /// <summary>
    /// Метод расширение для обертки конструкции резрешения интерфейса, выполнения с реализацией действий и высвобождения ресурсов
    /// Использовать только для компонентов с жизненым циклом Transient
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="container"></param>
    /// <param name="key"></param>
    /// <param name="arguments"></param>
    /// <param name="action"></param>
    public static void UsingForResolved<T>(
      this IWindsorContainer container,
      string key,
      object arguments,
      Action<IWindsorContainer, T> action)
      where T : class
    {
      T handler = container.Resolve<T>(key, Arguments.FromProperties(arguments));
      WindsorExtensions.HandlerActionInvoker<T>(container, handler, action);
    }

    /// <summary>
    /// Метод расширение для обертки конструкции резрешения интерфейса, выполнения с реализацией действий и высвобождения ресурсов
    /// Использовать только для компонентов с жизненым циклом Transient
    /// </summary>
    /// <param name="container"></param>
    /// <param name="serviceType"></param>
    /// <param name="arguments"></param>
    /// <param name="action"></param>
    public static void UsingForResolved(
      this IWindsorContainer container,
      Type serviceType,
      object arguments,
      Action<IWindsorContainer, object> action)
    {
      object handler = container.Resolve(serviceType, Arguments.FromProperties(arguments));
      WindsorExtensions.HandlerActionInvoker<object>(container, handler, action);
    }

    /// <summary>
    /// Метод расширение для обертки конструкции резрешения интерфейса, выполнения с реализацией действий и высвобождения ресурсов
    /// Использовать только для компонентов с жизненым циклом Transient
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="container"></param>
    /// <param name="arguments"></param>
    /// <param name="action"></param>
    public static void UsingForResolved<T>(
      this IWindsorContainer container,
      object arguments,
      Action<IWindsorContainer, T> action)
      where T : class
    {
      T handler = container.Resolve<T>(Arguments.FromProperties(arguments));
      WindsorExtensions.HandlerActionInvoker<T>(container, handler, action);
    }

    /// <summary>
    /// Метод расширение для обертки конструкции резрешения интерфейса, выполнения с реализацией действий и высвобождения ресурсов
    /// Использовать только для компонентов с жизненым циклом Transient
    /// </summary>
    public static void UsingForResolved<T>(
      this IWindsorContainer container,
      string key,
      Action<IWindsorContainer, T> action)
      where T : class
    {
      T handler = container.Resolve<T>(key);
      WindsorExtensions.HandlerActionInvoker<T>(container, handler, action);
    }

    /// <summary>
    /// Метод расширение для обертки конструкции резрешения интерфейса, выполнения с реализацией действий и высвобождения ресурсов
    /// Использовать только для компонентов с жизненым циклом Transient
    /// </summary>
    public static void UsingForResolved<T>(
      this IWindsorContainer container,
      Action<IWindsorContainer, T> action)
      where T : class
    {
      T handler = container.Resolve<T>();
      WindsorExtensions.HandlerActionInvoker<T>(container, handler, action);
    }

    /// <summary>
    /// Метод расширение для обертки конструкции резрешения интерфейса, выполнения с реализацией действий и высвобождения ресурсов
    /// Использовать только для компонентов с жизненым циклом Transient
    /// </summary>
    public static void UsingForResolvedAll<T>(
      this IWindsorContainer container,
      object arguments,
      Action<IWindsorContainer, IEnumerable<T>> action)
      where T : class
    {
      T[] handlers = container.ResolveAll<T>(Arguments.FromProperties(arguments));
      WindsorExtensions.HandlersActionInvoker<T>(container, (IEnumerable<T>) handlers, action);
    }

    /// <summary>
    /// Метод расширение для обертки конструкции резрешения интерфейса, выполнения с реализацией действий и высвобождения ресурсов
    /// Использовать только для компонентов с жизненым циклом Transient
    /// </summary>
    public static void UsingForResolvedAll<T>(
      this IWindsorContainer container,
      Action<IWindsorContainer, IEnumerable<T>> action)
      where T : class
    {
      T[] handlers = container.ResolveAll<T>();
      WindsorExtensions.HandlersActionInvoker<T>(container, (IEnumerable<T>) handlers, action);
    }

    private static void HandlerActionInvoker<T>(
      IWindsorContainer container,
      T handler,
      Action<IWindsorContainer, T> action)
      where T : class
    {
      try
      {
        action(container, handler);
      }
      finally
      {
        container.Release((object) handler);
      }
    }

    private static void HandlersActionInvoker<T>(
      IWindsorContainer container,
      IEnumerable<T> handlers,
      Action<IWindsorContainer, IEnumerable<T>> action)
      where T : class
    {
      try
      {
        action(container, handlers);
      }
      finally
      {
        foreach (T handler in handlers)
          container.Release((object) handler);
      }
    }

    /// <summary>
    /// Возвращает IDisposable обертку, которая будет освобождать полученный компонент из контейнера
    /// </summary>
    /// <param name="container">
    /// <see cref="T:Castle.Windsor.IWindsorContainer" />
    /// </param>
    /// <param name="components">Коллекция компонентов, полученных из контейнера</param>
    /// <returns>IDisposable</returns>
    public static IDisposable Using(
      this IWindsorContainer container,
      params object[] components)
    {
      return (IDisposable) new DisposableWrapper(container, components);
    }

    public sealed class ResolvedDisposable<T> : IDisposable
    {
      private IWindsorContainer _container;

      internal ResolvedDisposable(IWindsorContainer container, Type serviceType = null, Arguments args = null)
      {
        this._container = container;
        IWindsorContainer container1 = this._container;
        Type service = serviceType;
        if ((object) service == null)
          service = typeof (T);
        Arguments arguments = args;
        this.Service = (T) container1.Resolve(service, arguments);
      }

      public T Service { get; private set; }

      void IDisposable.Dispose()
      {
        if (this._container == null)
          return;
        this._container.Release((object) this.Service);
        if (this.Service is IDisposable service)
          service.Dispose();
        this.Service = default (T);
        this._container = (IWindsorContainer) null;
      }
    }

    public class ResolvedDisposables<T> : 
      IReadOnlyCollection<T>,
      IEnumerable<T>,
      IEnumerable,
      IDisposable
    {
      private IWindsorContainer _container;
      private T[] _services;

      internal ResolvedDisposables(IWindsorContainer container, Type serviceType = null, Arguments args = null)
      {
        this._container = container;
        IWindsorContainer container1 = this._container;
        Type service = serviceType;
        if ((object) service == null)
          service = typeof (T);
        Arguments arguments = args;
        this._services = container1.ResolveAll(service, arguments).Cast<T>().ToArray<T>();
      }

      void IDisposable.Dispose()
      {
        if (this._container == null)
          return;
        foreach (T service in this._services)
        {
          this._container.Release((object) service);
          if (service is IDisposable disposable)
            disposable.Dispose();
        }
        this._services = (T[]) null;
        this._container = (IWindsorContainer) null;
      }

      public IEnumerator<T> GetEnumerator() => this._services.Cast<T>().GetEnumerator();

      IEnumerator IEnumerable.GetEnumerator() => this._services.GetEnumerator();

      public int Count => this._services.Length;
    }
  }