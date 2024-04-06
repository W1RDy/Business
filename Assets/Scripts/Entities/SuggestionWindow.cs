using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SuggestionWindow : Window
{
    [SerializeField] private TextMeshProUGUI _suggestionText;
    [SerializeField] private TextMeshProUGUI _suggestionTimeText;

    private SuggestionViewController _suggestionViewController;
    private Suggestion _suggestion;

    public void SetSuggestion(Suggestion suggestion)
    {
        if (_suggestionViewController == null) _suggestionViewController = new SuggestionViewController(_suggestionText, _suggestionTimeText);
        _suggestion = suggestion;

        _suggestionViewController.SetSuggestion(suggestion);
    }

    public Suggestion GetSuggestion()
    {
        return _suggestion;
    }
}

public class SuggestionViewController
{
    private TextMeshProUGUI _suggestionText;
    private TextMeshProUGUI _suggestionTimeText;

    public SuggestionViewController(TextMeshProUGUI suggestionText, TextMeshProUGUI suggestionTimeText)
    {
        _suggestionText = suggestionText;
        _suggestionTimeText = suggestionTimeText;
    }

    public void SetSuggestion(Suggestion suggestion)
    {
        _suggestionText.text = suggestion.Text;
        if (suggestion is SkipTimeSuggestion skipTimeSuggestion) _suggestionTimeText.text = "+" + skipTimeSuggestion.Time;
    }
}
