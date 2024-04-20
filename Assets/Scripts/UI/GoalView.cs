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

    public GoalView(TextMeshProUGUI titleText, Image timeProgressImage, TextMeshProUGUI remainingTimeText, TextMeshProUGUI rewardText, TextMeshProUGUI remainingQuality)
    {
        _titleText = titleText;

        _timeProgressImage = timeProgressImage;
        _remainingTimeText = remainingTimeText;

        _rewardText = rewardText;

        _remainingQuality = remainingQuality;
    }

    public void SetView(int id, int cost, int remainTime, int time, GoodsType goodsType)
    {
        SetId(id);
        SetCost(cost);
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
        _remainingTimeText.text = remainTime + " days remain";

        var timeProgress = (float)remainTime / time;

        _timeProgressImage.fillAmount = timeProgress;

        if (timeProgress > 0.6f) _timeProgressImage.color = Color.green;
        else if (timeProgress > 0.3f) _timeProgressImage.color = Color.yellow;
        else _timeProgressImage.color = Color.red;
    }

    public void SetQualityText(GoodsType goodsType)
    {
        string qualityText;
        if (goodsType == GoodsType.LowQuality) qualityText = "low +";
        else if (goodsType == GoodsType.MediumQuality) qualityText = "medium +";
        else qualityText = "high";

        _remainingQuality.text = qualityText;
    }
}
