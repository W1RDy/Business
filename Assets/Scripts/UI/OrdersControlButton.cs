using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class OrdersControlButton : CustomButton
{
    [SerializeField] protected Order _order;

    [SerializeField] protected TextMeshProUGUI _stateText;

    protected virtual void SetText(string text)
    {
        _stateText.text = text;
    }
}
