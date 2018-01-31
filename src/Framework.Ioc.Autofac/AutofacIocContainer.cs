using System;
using System.Collections.Generic;
using Autofac;

namespace Framework.Ioc.Autofac
{
    public class AutofacIocContainer : IIocContainer
    {
        private readonly IContainer _container;

        public AutofacIocContainer(IContainer container)
        {
            _container = container;
        }

        public object Get(Type type)
        {
            return _container.Resolve(type);
        }

        public T TryGet<T>()
        {
            T instance;
            if (_container.TryResolve(out instance))
                return instance;

            return default(T);
        }

        public T Get<T>()
        {
            return _container.Resolve<T>();
        }

        public T Get<T>(string name, string value)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll<T>()
        {
            throw new NotImplementedException();
        }

        public void Inject(object item)
        {
            throw new NotImplementedException();
        }
    }
}
