using CoinsCounter;
using UnityEngine;

[CreateAssetMenu(fileName = "SkipTimeAndWasteCoinsSuggestion", menuName = "Suggestions/New Skip Time And Waste Coins Suggestion")]
public class SkipTimeAndWasteCoinsSuggestion : SkipTimeSuggestion, IEventWithCoinsParameters
{
    private int _coins;
    public int CoinsRequirements => _coins;

    private HandsCoinsCounter _coinsCounter;

    public void SetCoinsParameters(int coins)
    {
        _coins = coins;
        if (_coinsCounter == null) _coinsCounter = ServiceLocator.Instance.Get<HandsCoinsCounter>();
    }

    public override void Apply()
    {
        base.Apply();
        _coinsCounter.RemoveCoins(_coins);
    }
}