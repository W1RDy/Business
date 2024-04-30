﻿using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoalView
{
    private TextMeshProUGUI _titleText;

    private Image _timeProgressImage;
    private TextMeshProUGUI _remainingTimeText;
    private TextMeshProUGUI _remainingQuality;

    private TextMeshProUGUI _rewardText;

    private DeviceService _deviceService;

    public GoalView(TextMeshProUGUI titleText, Image timeProgressImage, TextMeshProUGUI remainingTimeText, TextMeshProUGUI rewardText, TextMeshProUGUI remainingQuality, DeviceService deviceService)
    {
        _titleText = titleText;

        _timeProgressImage = timeProgressImage;
        _remainingTimeText = remainingTimeText;

        _rewardText = rewardText;

        _remainingQuality = remainingQuality;

        _deviceService = deviceService;
    }

    public void SetView(int id, int cost, int remainTime, int time, GoodsType goodsType)
    {
        if (_titleText != null) SetId(id);
        if (_rewardText != null) SetCost(cost);
        SetTime(remainTime, time);
        SetQualityText(goodsType);
    }

    public void SetId(int id)
    {
        _titleText.text = id.ToString();
    }

    public void SetCost(int cost)
    {
        _rewardText.text = cost.ToString();
    }

    public void SetTime(int remainTime, int time)
    {
        var text = _deviceService.IsDesktop ? LocalizationManager.GetTranslation("Days remain") : LocalizationManager.GetTranslation("Days");
        _remainingTimeText.text = remainTime + " " + text;

        var timeProgress = (float)remainTime / time;

        _timeProgressImage.fillAmount = timeProgress;

        if (timeProgress > 0.6f) _timeProgressImage.color = Color.green;
        else if (timeProgress > 0.3f) _timeProgressImage.color = Color.yellow;
        else _timeProgressImage.color = Color.red;
    }

    public void SetQualityText(GoodsType goodsType)
    {
        string qualityText;
        if (goodsType == GoodsType.LowQuality) qualityText = LocalizationManager.GetTranslation("Low") + " +";
        else if (goodsType == GoodsType.MediumQuality) qualityText = LocalizationManager.GetTranslation("Medium") + " +";
        else qualityText = LocalizationManager.GetTranslation("High");

        _remainingQuality.text = qualityText;
    }
}
