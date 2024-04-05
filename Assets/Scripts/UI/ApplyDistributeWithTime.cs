using TMPro;
using UnityEngine;

public class ApplyDistributeWithTime : CustomButton
{
    [SerializeField] private CoinsDistributor _coinsDistributor;

    [SerializeField] private int _time;
    [SerializeField] private TextMeshProUGUI _timeText;

    private void Awake()
    {
        if (_timeText != null) SetTime();
    }

    protected override void ClickCallback()
    {
        _buttonService.DistributeCoins(_time, _coinsDistributor);
    }

    private void SetTime()
    {
        _timeText.text = "+" + _time;
    }
}