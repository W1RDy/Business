using CoinsCounter;
using UnityEngine;

[CreateAssetMenu(fileName = "SkipTimeAndWasteCoinsSuggestion", menuName = "Suggestions/New Skip Time And Waste Coins Suggestion")]
public class SkipTimeAndWasteCoinsSuggestion : SkipTimeSuggestion, IEventWithCoinsParameters
{
    private int _coins;
    public int CoinsRequirements => _coins;

    private WasteCoinsHandler _wasteCoinsHandler;

    public void SetCoinsParameters(int coins)
    {
        _coins = coins;
        if (_wasteCoinsHandler == null) _wasteCoinsHandler = new WasteCoinsHandler();
    }

    public override void Apply()
    {
        base.Apply();
        _wasteCoinsHandler.WasteCoins(this);
    }
}
