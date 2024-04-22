using I2.Loc;
using TMPro;
using UnityEngine;

public class RestartButton : CustomButton
{
    [SerializeField] private TextMeshProUGUI _buttonText;

    public override void Init()
    {
        base.Init();
        SetText("Restart");
    }

    protected override void ClickCallback()
    {
        base.ClickCallback();
        _buttonService.RestartGame();
    }

    private void SetText(string key)
    {
        _buttonText.text = LocalizationManager.GetTranslation(key);
    }
}