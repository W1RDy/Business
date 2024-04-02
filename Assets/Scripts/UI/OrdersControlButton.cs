using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class OrdersControlButton : CustomButton
{
    [SerializeField] protected MonoBehaviour _orderClass;

    [SerializeField] protected TextMeshProUGUI _stateText;

    protected IOrder _order;

    protected override void Start()
    {
        if (_orderClass is IOrder order) _order = order;
        else throw new System.ArgumentException(_orderClass.name + " doesn't realize interface IOrder");
        base.Start();
    }

    protected virtual void SetText(string text)
    {
        _stateText.text = text;
    }
}
