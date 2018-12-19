using System;
using System.Collections.Generic;

namespace MovieUniverse.Services.Factory
{
    public class ServiceFactoryBase
    {
        private readonly Dictionary<Type, object> _serviceInstances = new Dictionary<Type, object>();
        public T CreateService<T>() where T : class, new()
        {
            Type type = typeof(T);
            if (_serviceInstances.ContainsKey(type))
            {
                return _serviceInstances[type] as T;
            }
            T instance = new T();
            _serviceInstances.Add(type, instance);
            return instance;
        }
    }
}
