using TMPro;
using UnityEngine;

public class OpenDistributeSuggestionButton : OpenButton
{
    private IOrderWithCallbacks _order;
    private IEventWithCoinsParameters _event;

    [SerializeField] private TextMeshProUGUI _buttonText;
    private DistributeSuggestionHandler _distributeSuggestionHandler;

    protected override void Awake()
    {
        base.Awake();
        _distributeSuggestionHandler = new DistributeSuggestionHandler(3);
        SetText("Get from bank");
    }

    public void InitVariant(IOrderWithCallbacks order)
    {
        _order = order;
    }

    public void InitVariant(IEventWithCoinsParameters _event)
    {
        this._event = _event;
    }

    private void SetText(string text)
    {
        _buttonText.text = text;
    }

    protected override void ClickCallback()
    {
        if (_order != null)
        {
            _distributeSuggestionHandler.OpenDistibuteSuggestion(_order);
        }
        else
        {
            _distributeSuggestionHandler.OpenDistibuteSuggestion(_event);
        }
    }
}