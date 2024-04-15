using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _qualityText;

    [SerializeField] private TextMeshProUGUI _indexText;

    [SerializeField] private Icon _icon;
    private IconComponentsRandomizer _iconRandomizer;

    [SerializeField] private Image[] _borders;

    [SerializeField] private Color _positiveColor;
    [SerializeField] private Color _negativeColor;

    private bool _isGoodBorders;

    public void SetView(int coinsValue, int timeValue, GoodsType goodsType, int id)
    {
        if (_iconRandomizer == null) _iconRandomizer = ServiceLocator.Instance.Get<IconComponentsRandomizer>();
        _indexText.text = "order " + id;
        SetCoins(coinsValue);
        SetTime(timeValue);
        SetQuality(goodsType);
        SetIcon();
        ChangeBorders(true);
    }

    public void SetCoins(int coinsValue)
    {
        _coinsText.text = coinsValue.ToString();
    }

    public void SetTime(int timeValue)
    {
        _timeText.text = timeValue.ToString();
    }   

    public void SetQuality(GoodsType goodsType)
    {
        string quality;
        if (goodsType == GoodsType.LowQuality) quality = "low +";
        else if (goodsType == GoodsType.MediumQuality) quality = "medium +";
        else quality = "high";

        _qualityText.text = quality;
    }

    public void SetIcon()
    {
        var iconComponents = _iconRandomizer.RandomizeComponents();
        _icon.SetNewIconImage(iconComponents);
    }

    public void ChangeBorders(bool isGoodView)
    {
        if (_isGoodBorders != isGoodView)
        {
            foreach (var border in _borders)
            {
                border.color = isGoodView ? _positiveColor : _negativeColor;
            }
            _isGoodBorders = isGoodView;
        }
    }
}
