using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderFactory : IFactory
{
    private const string Path = "Order";

    private Order _orderPrefab;

    private RectTransform _container;

    public OrderFactory(RectTransform container)
    {
        _container = container;
    }

    public void LoadResources()
    {
        _orderPrefab = Resources.Load<Order>(Path);
    }

    public MonoBehaviour Create()
    {
        return Create(Vector2.zero, Quaternion.identity, null);
    }

    public MonoBehaviour Create(Vector2 pos, Quaternion rotation, Transform parent)
    {
        var order = MonoBehaviour.Instantiate(_orderPrefab, pos, rotation, _container);

        order.transform.localRotation = rotation;
        order.transform.localPosition = Vector3.zero;

        var layoutRootForRebuild = order.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<RectTransform>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutRootForRebuild);

        return order;
    }
}
