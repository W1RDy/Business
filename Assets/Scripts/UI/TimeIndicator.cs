using I2.Loc;
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
        if (month == 0) _monthText.text = LocalizationManager.GetTranslation("Study month");
        else _monthText.text = month + " " + LocalizationManager.GetTranslation("Month");
    }
}
