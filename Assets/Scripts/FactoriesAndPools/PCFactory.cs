using UnityEngine;

public class PCFactory : IFactory
{
    private const string Path = "PC";

    private PC _pcPrefab;

    private RectTransform _container;

    public PCFactory(RectTransform container)
    {
        _container = container;
    }

    public void LoadResources()
    {
        _pcPrefab = Resources.Load<PC>(Path);
    }

    public MonoBehaviour Create()
    {
        return Create(Vector2.zero, Quaternion.identity, null);
    }

    public MonoBehaviour Create(Vector2 pos, Quaternion rotation, Transform parent)
    {
        var goods = MonoBehaviour.Instantiate(_pcPrefab, pos, rotation, _container);

        goods.transform.localRotation = rotation;
        goods.transform.localPosition = Vector3.zero;

        return goods;
    }
}
