using System;
using System.Collections.Generic;

namespace Context
{
    public class ServiceProvider
    {
        private readonly Dictionary<Type, object> _services = new();

        public void Register<T>(T service)
        {
            _services[typeof(T)] = service;
        }

        public T Get<T>()
        {
            if (_services.TryGetValue(typeof(T), out var service))
                return (T)service;

            throw new Exception($"Service {typeof(T)} not found!");
        }
    }
}
