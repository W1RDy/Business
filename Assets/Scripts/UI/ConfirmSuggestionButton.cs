using TMPro;
using UnityEngine;

public class ConfirmSuggestionButton : CloseButton
{
    [SerializeField] private TextMeshProUGUI _buttonText;

    protected override void Awake()
    {
        base.Awake();
        SetText("Ok");
    }

    protected override void ClickCallback()
    {
        _buttonService.CloseWindow(WindowType.SuggestionWindow);
        _buttonService.ConfirmSuggestion((_window as SuggestionWindow).GetSuggestion());
    }

    protected void SetText(string text)
    {
        _buttonText.text = text;
    }
}