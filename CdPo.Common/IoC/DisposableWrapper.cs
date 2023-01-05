using System.Collections;

using Castle.Windsor;

namespace CdPo.Common.IoC;

internal class DisposableWrapper : IDisposable
{
    private readonly object[] _components;
    private readonly IWindsorContainer _container;

    public DisposableWrapper(IWindsorContainer container, params object[] components)
    {
        this._container = container;
        this._components = components;
    }

    public void Dispose()
    {
        if (this._components == null || !((IEnumerable<object>) this._components).Any<object>())
            return;
        foreach (object component in this._components)
        {
            if (component is IEnumerable enumerable)
            {
                foreach (object instance in enumerable)
                    this._container.Release(instance);
            }
            else if (component != null)
                this._container.Release(component);
        }
    }
}