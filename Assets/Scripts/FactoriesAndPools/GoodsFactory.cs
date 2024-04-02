using UnityEngine;

public class GoodsFactory : BaseFactory
{
    private const string Path = "Goods";

    public GoodsFactory(RectTransform container) : base(container)
    {

    }

    public override void LoadResources()
    {
        if (_prefab == null) _prefab = Resources.Load<Goods>(Path);
    }
}
