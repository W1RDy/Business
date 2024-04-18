﻿using TMPro;
using UnityEngine;

public class TutorialPartWithText : TutorialSegmentPart
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private string _message;

    private string _defaultMessage;
    private bool _startTextEnabledState;

    public override void Activate()
    {
        _startTextEnabledState = _text.transform.parent.gameObject.activeSelf;
        _defaultMessage = _text.text;
        _text.text = _message;
        _text.transform.parent.gameObject.SetActive(true);
    }

    public override void Deactivate()
    {
        _text.text = _defaultMessage;
        _text.transform.parent.gameObject.SetActive(_startTextEnabledState);
    }

    public override bool ConditionCompleted()
    {
        return !_text.gameObject.activeInHierarchy;
    }
}
