using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeliveryView : MonoBehaviour
{
    private TextMeshProUGUI _costText;
    private TextMeshProUGUI _timeText;

    private TextMeshProUGUI _titleText;
    private TextMeshProUGUI _descriptionText;

    public DeliveryView(TextMeshProUGUI costText, TextMeshProUGUI timeText, TextMeshProUGUI titleText, TextMeshProUGUI descriptionText)
    {
        _costText = costText;
        _timeText = timeText;

        _titleText = titleText;
        _descriptionText = descriptionText;
    }

    public void SetView(int cost, int time, string title, string description)
    {
        _costText.text = "- " + cost.ToString();
        _timeText.text = "+ " + time.ToString();

        _titleText.text = title;
        _descriptionText.text = description;
    }
}
