using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderFactory : BaseFactory
{
    private const string Path = "Order";

    public OrderFactory(RectTransform container) : base(container)
    {

    }

    public override void LoadResources()
    {
        if (_prefab == null) _prefab = Resources.Load<Order>(Path);
    }

    public override MonoBehaviour Create(Vector2 pos, Quaternion rotation, Transform parent)
    {
        var order = base.Create(pos, rotation, parent);

        var layoutRootForRebuild = order.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<RectTransform>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutRootForRebuild);

        return order;
    }
}

public class TutorialOrderFactory : OrderFactory
{
    private const string Path = "TutorialOrder";

    public TutorialOrderFactory(RectTransform container) : base(container)
    {

    }

    public override void LoadResources()
    {
        if (_prefab == null) _prefab = Resources.Load<Order>(Path);
    }
}

public class DeliveryOrderFactory : BaseFactory
{
    private const string Path = "DeliveryOrder";

    public DeliveryOrderFactory(RectTransform container) : base (container)
    {

    }

    public override void LoadResources()
    {
        _prefab = Resources.Load<DeliveryOrder>(Path);
    }
}