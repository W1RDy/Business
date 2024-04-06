using TMPro;
using UnityEngine;

public class OrderView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _qualityText;

    [SerializeField] private ApplyOrderButton _applyOrderButton;
    [SerializeField] private SendOrderButton _sendOrderButton;

    [SerializeField] private TextMeshProUGUI _indexText;

    private ButtonChanger _buttonChanger;

    private void Awake()
    {
        _buttonChanger = new ButtonChanger(_applyOrderButton, _sendOrderButton);
    }

    public void SetView(int coinsValue, int timeValue, GoodsType goodsType, int id)
    {
        _indexText.text = "order " + id;
        SetCoins(coinsValue);
        SetTime(timeValue);
        SetQuality(goodsType);
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
        if (goodsType == GoodsType.LowQuality) quality = "low";
        else if (goodsType == GoodsType.MediumQuality) quality = "medium";
        else quality = "high";

        _qualityText.text = quality;
    }

    public void ChangeApplyState(bool isApplied)
    {
        _applyOrderButton.ChangeState(isApplied);
    }

    public void ChangeButton(bool activateDefaultButton)
    {
        _buttonChanger.ChangeButtons(activateDefaultButton);
    }
}
