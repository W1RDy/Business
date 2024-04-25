using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Pool<T> : IPool<T>, IService, IResetable where T : MonoBehaviour, IPoolElement<T>
{
    protected int _startPoolSize;

    protected List<IPoolElement<T>> _elementsList = new List<IPoolElement<T>>();

    protected IFactory _factory;
    private RectTransform _poolContainer;
    private Transform _parent;

    private ResetContoroller _resetContoroller;

    public Pool(IFactory factory, RectTransform poolContainer, Transform parent, int startPoolSize)
    {
        _poolContainer = poolContainer;
        _parent = parent;

        _factory = factory;
        _factory.LoadResources();

        _startPoolSize = startPoolSize;

        _resetContoroller = ServiceLocator.Instance.Get<ResetContoroller>();
        _resetContoroller.AddResetable(this);
    }

    public virtual void Init()
    {
        for (int i = 0; i < _startPoolSize; i++)
        {
            var poolElement = Create();

            if (poolElement == null) throw new System.ArgumentException(typeof(T) + " doesn't realize IPoolElement interface!");
        }
    }

    public virtual T Create()
    {
        if (_elementsList.Count == _startPoolSize) _startPoolSize++;

        var element = _factory.Create() as T;

        _elementsList.Add(element);
        return element;
    }

    public virtual T Get()
    {
        foreach (var element in _elementsList)
        {
            if (element.IsFree)
            {
                ChangeParentWithScaleSaving(element.Element, _parent);

                element.Activate();
                return element.Element;
            }
        }

        var newElement = Create();
        newElement.transform.SetParent(_parent);
        newElement.Activate();

        return newElement;
    }

    public virtual void Release(T element)
    {
        if (element is Order order) Debug.Log("Release");
        element.Release();
        ChangeParentWithScaleSaving(element, _poolContainer);
    }

    private void ChangeParentWithScaleSaving(T element, Transform parent)
    {
        var scale = element.Element.transform.localScale;
        element.Element.transform.SetParent(parent);
        element.Element.transform.localScale = scale;
    }

    public void Reset()
    {
        foreach (var element in _elementsList)
        {
            if (!element.IsFree) Release(element.Element);
        }
    }
}