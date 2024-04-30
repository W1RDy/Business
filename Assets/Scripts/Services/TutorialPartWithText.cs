using I2.Loc;
using TMPro;
using UnityEngine;

public class TutorialPartWithText : TutorialSegmentPart
{
    [SerializeField] private DeviceService _deviceService;

    [SerializeField] private string _messageKey;
    private TextMeshProUGUI _text;

    private string _defaultMessage;
    private float _defaultFontSize;
    private bool _startTextEnabledState;

    public override void Activate()
    {
        SetTutorialTextPlace();
        _startTextEnabledState = _text.transform.parent.gameObject.activeSelf;
        _defaultMessage = _text.text;

        if (_messageKey.StartsWith("Distribute"))
        {
            _defaultFontSize = _text.fontSize;
            _text.fontSize -= 10f;
        }

        _text.text = LocalizationManager.GetTranslation("Tutorial/" + _messageKey);
        _text.transform.parent.gameObject.SetActive(true);
    }

    public override void Deactivate()
    {
        if (_messageKey.StartsWith("Distribute"))
        {
            _text.fontSize = _defaultFontSize;
        }

        _text.text = _defaultMessage;
        _text.transform.parent.gameObject.SetActive(_startTextEnabledState);
    }

    public override bool ConditionCompleted()
    {
        return !_text.gameObject.activeInHierarchy;
    }

    private void SetTutorialTextPlace()
    {
        if (_messageKey == "Order tutorial") _text = _deviceService.UILinksService._ordersTutorialText;
        else if (_messageKey == "Delivery tutorial") _text = _deviceService.UILinksService._deliveryTutorialText;
        else if (_messageKey == "Confirm delivery tutorial") _text = _deviceService.UILinksService._confirmDeliveryTutorialText;
        else if (_messageKey == "Goal tutorial") _text = _deviceService.UILinksService._sendTutorialText;
        else if (_messageKey == "Distribute coins tutorial") _text = _deviceService.UILinksService._distributeCoinsTutorialText;
    }
}
