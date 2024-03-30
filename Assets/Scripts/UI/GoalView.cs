﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoalView
{
    private TextMeshProUGUI _titleText;

    private Image _timeProgressImage;
    private TextMeshProUGUI _remainingTimeText;

    private TextMeshProUGUI _rewardText;

    public GoalView(TextMeshProUGUI titleText, Image timeProgressImage, TextMeshProUGUI remainingTimeText, TextMeshProUGUI rewardText)
    {
        _titleText = titleText;

        _timeProgressImage = timeProgressImage;
        _remainingTimeText = remainingTimeText;

        _rewardText = rewardText;
    }

    public void SetView(int id, int cost, int remainTime, int time)
    {
        SetId(id);
        SetCost(cost);
        SetTime(remainTime, time);
    }

    public void SetId(int id)
    {
        _titleText.text = "order " + id;
    }

    public void SetCost(int cost)
    {
        _rewardText.text = cost.ToString();
    }

    public void SetTime(int remainTime, int time)
    {
        _remainingTimeText.text = remainTime + " days remain";

        var timeProgress = (float)remainTime / time;

        _timeProgressImage.fillAmount = timeProgress;

        if (timeProgress > 0.6f) _timeProgressImage.color = Color.green;
        else if (timeProgress > 0.3f) _timeProgressImage.color = Color.yellow;
        else _timeProgressImage.color = Color.red;
    }
}
