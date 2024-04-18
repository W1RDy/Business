using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class OrdersControlButton : TutorialButton
{
    [SerializeField] protected MonoBehaviour _orderClass;

    [SerializeField] protected TextMeshProUGUI _stateText;

    protected IOrderWithCallbacks _order;

    public override void Init()
    {
        if (_orderClass)
        {
            if (_orderClass is IOrderWithCallbacks order) _order = order;
            else throw new System.ArgumentException(_orderClass.name + " doesn't realize interface IOrder");
        }

        base.Init();
    }

    protected virtual void SetText(string text)
    {
        _stateText.text = text;
    }
}
