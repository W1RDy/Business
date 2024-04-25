using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsInitializator : MonoBehaviour, IService
{
    [SerializeField] ObjectForInitialization[] _hiddenObjectsForInit;
    private List<IInitializable> _initializables = new List<IInitializable>();

    private void Awake()
    {
        ServiceLocator.Instance.ServiceRegistered += InitObjects;
    }

    private void InitObjects()
    {
        _initializables.AddRange(_hiddenObjectsForInit);
        foreach (var initializable in _initializables)
        {
            initializable.Init();
        }
    }

    public void Initialize(IInitializable initializable)
    {
        if (ServiceLocator.Instance.IsRegistered) initializable.Init();
        else
        {
            _initializables.Add(initializable);
        }
    }

    private void OnDestroy()
    {
        ServiceLocator.Instance.ServiceRegistered -= InitObjects;
    }
}

public abstract class ObjectForInitialization : MonoBehaviour, IInitializable
{
    private bool _isAddToInit;
    private Action InitDelegate;

    protected virtual void Awake()
    {
        if (!_isAddToInit)
        {
            if (ServiceLocator.Instance.IsRegistered) AddToInit();
            else
            {
                InitDelegate = () =>
                {
                    AddToInit();
                    ServiceLocator.Instance.ServiceRegistered -= InitDelegate;
                };
                ServiceLocator.Instance.ServiceRegistered += InitDelegate;
            }
            _isAddToInit = true;
        }

    }

    protected void AddToInit()
    {
        var initializator = ServiceLocator.Instance.Get<ObjectsInitializator>();
        initializator.Initialize(this);
    }

    public virtual void Init()
    {
        _isAddToInit = true;
    }

    protected virtual void OnDestroy()
    {
        ServiceLocator.Instance.ServiceRegistered -= InitDelegate;
    }
}

public abstract class ObjectForInitializationWithChildren : ObjectForInitialization, IInitializable
{
    [SerializeField] ObjectForInitialization[] _objectForInitializations;

    public override void Init()
    {
        base.Init();
        foreach (var obj in _objectForInitializations)
        {
            obj.Init();
        }
    }
}

public abstract class ClassForInitialization : IInitializable
{
    public ClassForInitialization()
    {
        var initializator = ServiceLocator.Instance.Get<ObjectsInitializator>();
        initializator.Initialize(this);
    }

    public abstract void Init();
}
