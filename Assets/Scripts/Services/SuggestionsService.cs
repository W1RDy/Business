using System.Collections.Generic;
using UnityEngine;

public class SuggestionsService : MonoBehaviour, IService
{
    [SerializeField] private Suggestion[] _suggestions;
    private Dictionary<ConfirmType, Suggestion> _suggestionsDict = new Dictionary<ConfirmType, Suggestion>();

    private void Awake()
    {
        InitDictionary();
    }

    private void InitDictionary()
    {
        foreach (var suggestion in _suggestions)
        {
            _suggestionsDict.Add((ConfirmType)int.Parse(suggestion.ID), Instantiate(suggestion));
        }
    }

    public Suggestion GetSuggestion(ConfirmType confirmType)
    {
        return _suggestionsDict[confirmType];
    }
}
