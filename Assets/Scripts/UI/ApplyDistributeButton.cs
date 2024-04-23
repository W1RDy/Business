using I2.Loc;
using TMPro;
using UnityEngine;

public class ApplyDistributeButton : CustomButton
{
    [SerializeField] private CoinsDistributor _coinsDistributor;

    [SerializeField] private TextMeshProUGUI _buttonText;

    public override void Init()
    {
        base.Init();
        SetText("Apply");
    }

    protected override void ClickCallback()
    {
        _buttonService.DistributeCoins(_coinsDistributor);
    }

    private void SetText(string textKey)
    {
        _buttonText.text = LocalizationManager.GetTranslation(textKey);
    }
}