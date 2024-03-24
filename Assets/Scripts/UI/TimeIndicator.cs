using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeIndicator : MonoBehaviour
{
    private int _maxTimeValue;

    [SerializeField] private Image _fillImage;
    [SerializeField] private TextMeshProUGUI _monthText;

    public void Init(int maxTime)
    {
        _maxTimeValue = maxTime;
    }

    public void SetTime(int time)
    {
        _fillImage.fillAmount = (float)time / _maxTimeValue;
    }

    public void UpdateMonth(int month)
    {
        _monthText.text = month + " month";
    }
}
