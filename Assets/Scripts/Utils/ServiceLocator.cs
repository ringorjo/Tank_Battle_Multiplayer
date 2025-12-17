using System;
using System.Collections.Generic;

public class ServiceLocator
{
    private Dictionary<Type, IService> _serviceMap = new Dictionary<Type, IService>();
    private static ServiceLocator _instance;

    public ServiceLocator()
    {
        _serviceMap = new Dictionary<Type, IService>();
    }

    public static ServiceLocator Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ServiceLocator();
            }
            return _instance;
        }
    }

    public void Register<T>(T service) where T : IService
    {
        Type serviceType = service.GetType();
        if (!_serviceMap.ContainsKey(serviceType))
            _serviceMap.Add(serviceType, service);
    }
    public void Unregister<T>(T service) where T : IService
    {
        Type serviceType = service.GetType();
        if (_serviceMap.ContainsKey(serviceType))
            _serviceMap.Remove(serviceType);
    }

    public T Get<T>() where T : IService
    {
        if (!_serviceMap.ContainsKey(typeof(T)))
        {
            throw new Exception("Service not exist");
        }
        return (T)_serviceMap[typeof(T)];

    }
}
