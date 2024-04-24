using CoinsCounter;
using System;

public class ContinueHandler : ClassForInitialization
{
    private GameController _gameController;

    private DifficultyController _difficultyController;

    private HandsCoinsCounter _handsCoinsCounter;
    private BankCoinsCounter _bankCoinsCounter;
    private WindowActivator _windowActivator;

    public ContinueHandler() : base() { }

    public override void Init()
    {
        _gameController = ServiceLocator.Instance.Get<GameController>();
        _difficultyController = ServiceLocator.Instance.Get<DifficultyController>();

        _handsCoinsCounter = ServiceLocator.Instance.Get<HandsCoinsCounter>();
        _bankCoinsCounter = ServiceLocator.Instance.Get<BankCoinsCounter>();

        _windowActivator = ServiceLocator.Instance.Get<WindowActivator>();
    }

    public void Continue()
    {
        _gameController.Continue();

        _handsCoinsCounter.SetCoins(RoundToTen(_difficultyController.StartCoinsInHands * _difficultyController.Difficulty));
        _bankCoinsCounter.SetCoins(RoundToTen(_difficultyController.StartCoinsInBank * _difficultyController.Difficulty));

        _windowActivator.DeactivateWindow(WindowType.Results);
    }

    private int RoundToTen(float value)
    {
        int intValue = (int)Math.Round(value, MidpointRounding.AwayFromZero);
        int roundedValue;
        if (value % 10 > 4) roundedValue = intValue - intValue % 10;
        else roundedValue = intValue + intValue % 10;

        return roundedValue;
    }
}