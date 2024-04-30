using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class TimeButton : CustomButton
{
    [SerializeField] private int _timeValue;

    [SerializeField] private TextMeshProUGUI _buttonText;

    public override void Init() 
    {
        base.Init();
        if (_buttonText != null) SetText("Skip day");
    }

    protected override void ClickCallback()
    {
        base.ClickCallback();
        _buttonService.TryAddTime(_timeValue);
    }

    private void SetText(string key)
    {
        _buttonText.text = LocalizationManager.GetTranslation(key);
    }
}
