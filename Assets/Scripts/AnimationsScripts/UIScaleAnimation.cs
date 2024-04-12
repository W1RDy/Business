using UnityEngine;

public abstract class UIScaleAnimation : UIAnimation
{
    protected Transform _transform;

    public void SetParametres(Transform transform)
    {
        _transform = transform;
    }
}
