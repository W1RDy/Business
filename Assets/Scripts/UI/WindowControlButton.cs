using System.Collections;
using UnityEngine;

public abstract class WindowControlButton : CustomButton
{
    [SerializeField] protected Window _window;

    protected WindowType _windowType;

    protected void Awake()
    {
        _windowType = _window.Type;
    }
}
