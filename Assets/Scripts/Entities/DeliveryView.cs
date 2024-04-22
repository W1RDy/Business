using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryView : MonoBehaviour
{
    private TextMeshProUGUI _costText;
    private TextMeshProUGUI _timeText;

    private TextMeshProUGUI _titleText;
    private TextMeshProUGUI _descriptionText;

    private Image _icon;

    public DeliveryView(TextMeshProUGUI costText, TextMeshProUGUI timeText, TextMeshProUGUI titleText, TextMeshProUGUI descriptionText, Image icon)
    {
        _costText = costText;
        _timeText = timeText;

        _titleText = titleText;
        _descriptionText = descriptionText;

        _icon = icon;
    }

    public void SetView(int cost, int time, string titleKey, string descriptionKey, Sprite icon)
    {
        _costText.text = "- " + cost.ToString();
        _timeText.text = "+ " + time.ToString();

        _titleText.text = LocalizationManager.GetTranslation(titleKey);
        _descriptionText.text = LocalizationManager.GetTranslation(descriptionKey);

        _icon.sprite = icon;
    }
}
