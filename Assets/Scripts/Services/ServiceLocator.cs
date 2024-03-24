using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    public static ServiceLocator Instance;

    private Dictionary<Type, IService> _services = new Dictionary<Type, IService>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void Register(IService service)
    {
        var type = service.GetType();
        if (!_services.ContainsKey(type))
        {
            _services.Add(type, service);
        }
        else throw new ArgumentException("Service with type " + type + " already exists!");
    }

    public void Unregister(IService service)
    {
        var type = service.GetType();
        if (_services.ContainsKey(type))
        {
            _services.Remove(type);
        }
        else throw new ArgumentException("Service with type " + type + " doesn't exist!");
    }

    public T Get<T>() where T : IService
    {
        var type = typeof(T);

        return (T)_services[type];
    }
}
