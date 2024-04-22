using I2.Loc;
using System;
using TMPro;

public class EventsViewController
{
    private TextMeshProUGUI _descriptionText;
    private TextMeshProUGUI _requirementTimeText;
    private TextMeshProUGUI _requirementCoinsText;

    public EventsViewController(TextMeshProUGUI descriptionText, TextMeshProUGUI requirementTimeText, TextMeshProUGUI requirementCoinsText)
    {
        _descriptionText = descriptionText;
        _requirementTimeText = requirementTimeText;
        _requirementCoinsText = requirementCoinsText;
    }

    public void SetEvent(IEvent entityEvent)
    {
        _descriptionText.text = LocalizationManager.GetTranslation("Events/" + entityEvent.Description);
        if (entityEvent is IEventWithTimeParameters eventWithTime)
        {
            _requirementTimeText.transform.parent.gameObject.SetActive(true);
            _requirementTimeText.text = "+" + eventWithTime.TimeRequirements;
        }
        else _requirementTimeText.transform.parent.gameObject.SetActive(false);

        if (entityEvent is IEventWithCoinsParameters eventWithCoins)
        {
            _requirementCoinsText.transform.parent.gameObject.SetActive(true);
            _requirementCoinsText.text = "-" + eventWithCoins.CoinsRequirements;
        }
        else _requirementCoinsText.transform.parent.gameObject.SetActive(false);
    }
}
