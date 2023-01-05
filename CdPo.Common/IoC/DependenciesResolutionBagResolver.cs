using Castle.Core;
using Castle.DynamicProxy;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;

using CdPo.Common.Extensions;
using CdPo.Common.Interfaces;

namespace CdPo.Common.IoC;

/// <summary>
  /// Резолвер для <see cref="T:BarsUp.IoC.IDependenciesResolutionBag" />
  /// </summary>
  internal class DependenciesResolutionBagResolver : ISubDependencyResolver
  {
    private readonly IWindsorContainer _container;
    private readonly DefaultKernel _kernel = new DefaultKernel();
    private readonly IWindsorContainer _resolutionContainer;

    public DependenciesResolutionBagResolver(IWindsorContainer container)
    {
      this._container = container;
      this._resolutionContainer = (IWindsorContainer) new WindsorContainer("dependencies-resolution-bag", (IKernel) this._kernel, (IComponentsInstaller) new DefaultComponentInstaller());
      this._container.AddChildContainer(this._resolutionContainer);
    }

    public bool CanResolve(
      CreationContext context,
      ISubDependencyResolver contextHandlerResolver,
      ComponentModel model,
      DependencyModel dependency)
    {
      return dependency.TargetType != (Type) null && dependency.TargetType.IsClass && TypeExtensions.Is<IDependenciesResolutionBag>(dependency.TargetType);
    }

    public object Resolve(
      CreationContext context,
      ISubDependencyResolver contextHandlerResolver,
      ComponentModel model,
      DependencyModel dependency)
    {
      if (this._container.Kernel.HasComponent(dependency.TargetType))
        return this._container.Kernel.Resolve(dependency.TargetType);
      if (this._resolutionContainer.Kernel.HasComponent(dependency.TargetType))
        return this._resolutionContainer.Kernel.Resolve(dependency.TargetType);
      using (this._kernel.OptimizeDependencyResolution())
      {
        Type type = dependency.TargetType;
        if (dependency.TargetType.IsInterface)
          type = new ProxyGenerator().CreateInterfaceProxyWithoutTarget(dependency.TargetType, (IInterceptor) new ResolverProxyInterceptor()).GetType();
        Component.For(dependency.TargetType).LifestyleTransient().ImplementedBy(type).RegisterIn<object>(this._resolutionContainer);
        return this._resolutionContainer.Resolve(dependency.TargetType);
      }
    }
  }