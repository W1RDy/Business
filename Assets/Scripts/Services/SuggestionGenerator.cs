using System;

public class SuggestionGenerator : IService
{
    private SuggestionWindow _suggestionWindow;
    private DistributeCoinWindow _distributeCoinsWindow;
    private SuggestionsService _suggestionsService;

    private Action InitDelegate;

    public SuggestionGenerator()
    {
        InitDelegate = () =>
        {
            var windowService = ServiceLocator.Instance.Get<WindowService>();
            _suggestionWindow = windowService.GetWindow(WindowType.SuggestionWindow) as SuggestionWindow;
            _distributeCoinsWindow = windowService.GetWindow(WindowType.DistributeSuggestionWindow) as DistributeCoinWindow;

            _suggestionsService = ServiceLocator.Instance.Get<SuggestionsService>();

            ServiceLocator.Instance.ServiceRegistered -= InitDelegate;
        };
        ServiceLocator.Instance.ServiceRegistered += InitDelegate;
        if (ServiceLocator.Instance.IsRegistered) InitDelegate.Invoke();
    }

    public Suggestion GenerateSuggestion(ConfirmType confirmType, int timeParameter, int coinsParameter)
    {
        var suggestion = _suggestionsService.GetSuggestion(confirmType);
        var window = confirmType == ConfirmType.DistributeCoins ? _distributeCoinsWindow : _suggestionWindow;

        if (suggestion is IEventWithTimeParameters skipTimeSuggestion) skipTimeSuggestion.SetTimeParameters(timeParameter);
        if (suggestion is IEventWithCoinsParameters skipCoinsSuggestion) skipCoinsSuggestion.SetCoinsParameters(coinsParameter);

        window.SetSuggestion(suggestion);
        return suggestion;
    }

    public Suggestion GenerateSuggestion(ConfirmType confirmType, int timeParameter)
    {
        return GenerateSuggestion(confirmType, timeParameter, 0);
    }
}