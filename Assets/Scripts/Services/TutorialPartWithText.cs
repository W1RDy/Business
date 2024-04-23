using I2.Loc;
using TMPro;
using UnityEngine;

public class TutorialPartWithText : TutorialSegmentPart
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private string _messageKey;

    private string _defaultMessage;
    private float _defaultFontSize;
    private bool _startTextEnabledState;

    public override void Activate()
    {
        _startTextEnabledState = _text.transform.parent.gameObject.activeSelf;
        _defaultMessage = _text.text;

        if (_messageKey.StartsWith("Distribute"))
        {
            _defaultFontSize = _text.fontSize;
            _text.fontSize = 28.5f;
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
}
