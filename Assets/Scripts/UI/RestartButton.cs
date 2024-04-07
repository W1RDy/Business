using TMPro;
using UnityEngine;

public class RestartButton : CustomButton
{
    [SerializeField] private TextMeshProUGUI _buttonText;

    protected override void Init()
    {
        base.Init();
        SetText("Restart");
    }

    protected override void ClickCallback()
    {
        base.ClickCallback();
        _buttonService.RestartGame();
    }

    private void SetText(string text)
    {
        _buttonText.text = text;
    }
}