using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Framework.Ioc.Autofac
{
    public class AutofacIocWebContainer : IIocContainer
    {
        public object Get(Type type)
        {
            return DependencyResolver.Current.GetService(type);
        }

        public T TryGet<T>()
        {
            try
            {
                return Get<T>();
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        public T Get<T>()
        {
            return DependencyResolver.Current.GetService<T>();
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