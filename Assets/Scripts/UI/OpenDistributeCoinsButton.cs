using TMPro;
using UnityEngine;

public class OpenDistributeCoinsButton : OpenButton
{
    [SerializeField] private TextMeshProUGUI _buttonText;

    protected override void Awake()
    {
        base.Awake();
        SetText("Get from bank");
    }

    private void SetText(string text)
    {
        _buttonText.text = text;
    }

    protected override void ClickCallback()
    {
        _buttonService.OpenWindow(WindowType.DistributeCoinsWindow);
    }
}