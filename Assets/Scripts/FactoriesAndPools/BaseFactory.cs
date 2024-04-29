using UnityEngine;

public abstract class BaseFactory : IFactory
{
    protected MonoBehaviour _prefab;
    protected bool _isDesktop;

    protected RectTransform _container;

    public BaseFactory(RectTransform container, bool isDesktop)
    {
        _container = container;
        _isDesktop = isDesktop;
        LoadResources();
    }

    public abstract void LoadResources();

    public virtual MonoBehaviour Create()
    {
        return Create(Vector2.zero, Quaternion.identity, null);
    }

    public virtual MonoBehaviour Create(Vector2 pos, Quaternion rotation, Transform parent)
    {
        var monoBeh = MonoBehaviour.Instantiate(_prefab, pos, rotation, _container);

        monoBeh.transform.localRotation = rotation;
        monoBeh.transform.localPosition = Vector3.zero;

        return monoBeh;
    }
}