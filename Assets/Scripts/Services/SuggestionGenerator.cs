using System;

public class SuggestionGenerator : IService
{
    private SuggestionWindow _suggestionWindow;
    private SuggestionsService _suggestionsService;

    private Action InitDelegate;

    public SuggestionGenerator()
    {
        InitDelegate = () =>
        {
            _suggestionWindow = ServiceLocator.Instance.Get<WindowService>().GetWindow(WindowType.SuggestionWindow) as SuggestionWindow;
            _suggestionsService = ServiceLocator.Instance.Get<SuggestionsService>();

            ServiceLocator.Instance.ServiceRegistered -= InitDelegate;
        };
        ServiceLocator.Instance.ServiceRegistered += InitDelegate;
        if (ServiceLocator.Instance.IsRegistered) InitDelegate.Invoke();
    }

    public Suggestion GenerateSuggestion(string id, int parameter1)
    {
        var suggestion = _suggestionsService.GetSuggestion(id);
        if (suggestion is SkipTimeSuggestion skipTimeSuggestion) skipTimeSuggestion.SetSuggestionParameters(parameter1);

        _suggestionWindow.SetSuggestion(suggestion);
        return suggestion;
    }
}