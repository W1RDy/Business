using System.Collections;
using UnityEngine;

public abstract class WindowControlButton : CustomButton
{
    [SerializeField] protected Window _window;

    protected WindowType _windowType;

    public override void Init()
    {
        base.Init();
        _windowType = _window.Type;
    }
}
