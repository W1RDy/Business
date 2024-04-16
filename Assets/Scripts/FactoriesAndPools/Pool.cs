using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Pool<T> : IPool<T>, IService where T : MonoBehaviour, IPoolElement<T>
{
    private int _startPoolSize;

    private List<IPoolElement<T>> _elementsList = new List<IPoolElement<T>>();

    private IFactory _factory;
    private RectTransform _poolContainer;
    private Transform _parent;

    public Pool(IFactory factory, RectTransform poolContainer, Transform parent, int startPoolSize)
    {
        _poolContainer = poolContainer;
        _parent = parent;

        _factory = factory;
        _factory.LoadResources();

        _startPoolSize = startPoolSize;
    }

    public void Init()
    {
        for (int i = 0; i < _startPoolSize; i++)
        {
            var poolElement = Create();

            if (poolElement == null) throw new System.ArgumentException(typeof(T) + " doesn't realize IPoolElement interface!");
        }
    }

    public T Create()
    {
        if (_elementsList.Count == _startPoolSize) _startPoolSize++;

        var element = _factory.Create() as T;

        _elementsList.Add(element);
        return element;
    }

    public T Get()
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

    public void Release(T element)
    {
        element.Release();
        ChangeParentWithScaleSaving(element, _poolContainer);
    }

    private void ChangeParentWithScaleSaving(T element, Transform parent)
    {
        if (element as IOrder != null || element as Goods != null || element as PC != null)
        {
            if (element.Element.transform.localScale.x < 1 || element.Element.transform.localScale.y < 1 || element.Element.transform.localScale.z < 1)
            {
                Debug.Log("Bug with scale");
                element.Element.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        var scale = element.Element.transform.localScale;
        element.Element.transform.SetParent(parent);
        element.Element.transform.localScale = scale;
    }
}