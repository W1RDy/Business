using UnityEngine;

public class GoodsFactory : BaseFactory
{
    private const string Path = "Goods";

    public GoodsFactory(RectTransform container, bool isDesktop) : base(container, isDesktop)
    {

    }

    public override void LoadResources()
    {
        if (_prefab == null && _isDesktop) _prefab = Resources.Load<Goods>(Path);
        else if (_prefab == null && !_isDesktop) _prefab = Resources.Load<Goods>(Path + "Mobile");
    }
}
