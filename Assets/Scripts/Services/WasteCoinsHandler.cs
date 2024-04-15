using CoinsCounter;

public class WasteCoinsHandler
{
    private HandsCoinsCounter _coinsCounter;
    private ResultsOfTheMonthService _resultsOfTheMonthService;

    public WasteCoinsHandler()
    {
        _coinsCounter = ServiceLocator.Instance.Get<HandsCoinsCounter>();
        _resultsOfTheMonthService = ServiceLocator.Instance.Get<ResultsOfTheMonthService>();
    }

    public void WasteCoins(int coins)
    {
        _coinsCounter.RemoveCoins(coins);
        _resultsOfTheMonthService.UpdateResults(0, -coins, 0, 0);
    }
}