using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class SuggestionWindow : Window
{
    [SerializeField] private TextMeshProUGUI _suggestionText;
    [SerializeField] private TextMeshProUGUI _suggestionTimeText;
    [SerializeField] private TextMeshProUGUI _sggestionCoinsText;

    private EventsViewController _suggestionViewController;
    private Suggestion _suggestion;

    public void SetSuggestion(Suggestion suggestion)
    {
        if (_suggestionViewController == null) _suggestionViewController = new EventsViewController(_suggestionText, _suggestionTimeText, _sggestionCoinsText);
        _suggestion = suggestion;

        _suggestionViewController.SetEvent(suggestion);
    }

    public Suggestion GetSuggestion()
    {
        return _suggestion;
    }
}
