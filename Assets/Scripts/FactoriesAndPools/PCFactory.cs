using UnityEngine;

public class PCFactory : BaseFactory
{
    private const string Path = "PC";

    public PCFactory(RectTransform container) : base(container)
    {

    }

    public override void LoadResources()
    {
        if (_prefab == null) _prefab = Resources.Load<PC>(Path);
    }
}
