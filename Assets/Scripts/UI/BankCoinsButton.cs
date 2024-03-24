using TMPro;
using UnityEngine;

public class BankCoinsButton : CustomButton
{
    [SerializeField] private int _coinsValue;

    [SerializeField] private TextMeshProUGUI _buttonText;

    protected override void Init()
    {
        base.Init();

        SetText();
    }

    protected override void ClickCallback()
    {
        _buttonSertvice.DebitCoinsFromBank(_coinsValue);
    }

    private void SetText()
    {
        _buttonText.text = "Debit " + _coinsValue + " coins";
    }
}