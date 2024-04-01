using UnityEngine;

public class GoodsFactory : IFactory
{
    private const string Path = "Goods";

    private Goods _goodsPrefab;

    private RectTransform _container;

    public GoodsFactory(RectTransform container)
    {
        _container = container;
    }

    public void LoadResources()
    {
        _goodsPrefab = Resources.Load<Goods>(Path);
    }

    public MonoBehaviour Create()
    {
        return Create(Vector2.zero, Quaternion.identity, null);
    }

    public MonoBehaviour Create(Vector2 pos, Quaternion rotation, Transform parent)
    {
        var goods = MonoBehaviour.Instantiate(_goodsPrefab, pos, rotation, _container);

        goods.transform.localRotation = rotation;
        goods.transform.localPosition = Vector3.zero;

        return goods;
    }
}
