using System;

public class ConfirmHandler
{
    private SuggestionGenerator _suggestionGenerator;

    private ButtonService _buttonService;

    private Action _rememberedAction;
    private Suggestion _suggestion;

    private Action InitDelegate;

    public ConfirmHandler()
    {
        InitDelegate = () =>
        {
            _buttonService = ServiceLocator.Instance.Get<ButtonService>();
            _suggestionGenerator = ServiceLocator.Instance.Get<SuggestionGenerator>();

            ServiceLocator.Instance.ServiceRegistered -= InitDelegate;
        };
        ServiceLocator.Instance.ServiceRegistered += InitDelegate;
        if (ServiceLocator.Instance.IsRegistered) InitDelegate.Invoke();
    }

    public void ConfirmAction(Action action, int skipTime)
    {
        RememberActionForSuggestion(action);
        OpenSuggestionWindow(skipTime);
    }

    private void RememberActionForSuggestion(Action action)
    {
        _rememberedAction = action;
    }

    public void OpenSuggestionWindow(int skipTime)
    {
        _suggestion = _suggestionGenerator.GenerateSuggestion("SkipTime", skipTime);

        _suggestion.Applied += ConfirmSuggestion;
        _suggestion.Skipped += CancelSuggestion;

        _buttonService.OpenWindow(WindowType.SuggestionWindow);
    }

    private void ConfirmSuggestion()
    {
        _rememberedAction?.Invoke();
        CancelSuggestion();
    }

    private void CancelSuggestion()
    {
        if (_rememberedAction != null)
        {
            _rememberedAction = null;

            if (_suggestion != null)
            {
                _suggestion.Applied -= ConfirmSuggestion;
                _suggestion.Skipped -= CancelSuggestion;
                _suggestion = null;
            }
        }
    }
}