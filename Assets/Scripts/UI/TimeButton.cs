using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class TimeButton : CustomButton
{
    [SerializeField] private int _timeValue;

    [SerializeField] private TextMeshProUGUI _buttonText;

    protected override void Init() 
    {
        base.Init();

        SetText();
    }

    protected override void ClickCallback()
    {
        base.ClickCallback();
        _buttonService.AddTime(_timeValue);
    }

    private void SetText()
    {
        _buttonText.text = "Add " + _timeValue + " days";
    }
}
