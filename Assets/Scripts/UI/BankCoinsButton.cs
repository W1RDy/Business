using System;
using TMPro;
using UnityEngine;

public class BankCoinsButton : CustomButton
{
    [SerializeField] private int _coinsValue;

    [SerializeField] private TextMeshProUGUI _buttonText;

    [SerializeField] private bool _isDebitButton;

    protected override void Init()
    {
        base.Init();

        SetText();
    }

    protected override void ClickCallback()
    {
        base.ClickCallback();
        if (_isDebitButton) _buttonService.DebitCoinsFromBank(_coinsValue);
        else _buttonService.PutCoinsOnBunk(_coinsValue);
    }

    private void SetText()
    {
        string text;
        if (_isDebitButton) text = "Debit " + _coinsValue + " coins";
        else text = "Put on bank " + _coinsValue + " coins";
        
        _buttonText.text = text;
    }
}
