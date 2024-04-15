using CoinsCounter;
using UnityEngine;

public class WasteCoinsHandler
{
    private HandsCoinsCounter _coinsCounter;
    private ResultsOfTheMonthService _resultsOfTheMonthService;

    public WasteCoinsHandler()
    {
        _coinsCounter = ServiceLocator.Instance.Get<HandsCoinsCounter>();
        _resultsOfTheMonthService = ServiceLocator.Instance.Get<ResultsOfTheMonthService>();
    }

    public void WasteCoins(IEventWithCoinsParameters coinsEvent)
    {
        if (coinsEvent as Suggestion != null) RemoveCoinsForOrder(coinsEvent.CoinsRequirements);
        else if (coinsEvent as ProblemConfig != null) WasteCoins(coinsEvent.CoinsRequirements);
    }

    private void WasteCoins(int coins)
    {
        Debug.Log("WasteCoins");
        _coinsCounter.RemoveCoins(coins);
        _resultsOfTheMonthService.UpdateResults(0, -coins, 0, 0);
    }

    private void RemoveCoinsForOrder(int coins)
    {
        _coinsCounter.RemoveCoins(coins);
        _resultsOfTheMonthService.UpdateResults(-coins, 0, 0, 0);
    }
}