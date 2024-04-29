using UnityEngine;

public class PCFactory : BaseFactory
{
    private const string Path = "PC";

    public PCFactory(RectTransform container, bool isDesktop) : base(container, isDesktop)
    {

    }

    public override void LoadResources()
    {
        if (_prefab == null && _isDesktop) _prefab = Resources.Load<PC>(Path);
        else if (_prefab == null && !_isDesktop) _prefab = Resources.Load<PC>(Path + "Mobile");
    }
}
