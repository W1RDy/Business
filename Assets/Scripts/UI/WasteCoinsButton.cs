using TMPro;
using UnityEngine;

public class WasteCoinsButton : CustomButton
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
        base.ClickCallback();
        _buttonService.WasteCoinsByProblems(_coinsValue);
    }

    public void SetCoinsValue(int value)
    {
        _coinsValue = value;
    }

    private void SetText()
    {
        _buttonText.text = "Ok";
    }
}
