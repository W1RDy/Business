using Unity.VisualScripting;
using UnityEngine;

public class ThrowOutButton : CustomButton
{
    [SerializeField] private MonoBehaviour _throwableObj;
    private IThrowable _throwable;

    protected override void Start()
    {
        base.Start();
        if (_throwableObj is IThrowable throwable) _throwable = throwable;
        else throw new System.ArgumentException(_throwableObj.name + " doesn't realize IThrowable interface!");
    }
    protected override void ClickCallback()
    {
        base.ClickCallback();
        CancelOrder();
    }

    public void CancelOrder()
    {
        _buttonService.ThrowOut(_throwable);
    }
}