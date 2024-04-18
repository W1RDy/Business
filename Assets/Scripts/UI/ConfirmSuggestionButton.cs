using TMPro;
using UnityEngine;

public class ConfirmSuggestionButton : CloseButton
{
    [SerializeField] private TextMeshProUGUI _buttonText;

    public override void Init()
    {
        base.Init();
        SetText("Ok");
    }

    protected override void ClickCallback()
    {
        base.ClickCallback();
        _buttonService.CloseWindow(WindowType.SuggestionWindow);
        _buttonService.ConfirmSuggestion((_window as SuggestionWindow).GetSuggestion());
    }

    protected void SetText(string text)
    {
        _buttonText.text = text;
    }
}