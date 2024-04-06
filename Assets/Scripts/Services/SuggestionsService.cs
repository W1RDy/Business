using System.Collections.Generic;
using UnityEngine;

public class SuggestionsService : MonoBehaviour, IService
{
    [SerializeField] private Suggestion[] _suggestions;
    private Dictionary<string, Suggestion> _suggestionsDict = new Dictionary<string, Suggestion>();

    private void Awake()
    {
        InitDictionary();
    }

    private void InitDictionary()
    {
        foreach (var suggestion in _suggestions)
        {
            _suggestionsDict.Add(suggestion.ID, Instantiate(suggestion));
        }
    }

    public Suggestion GetSuggestion(string id)
    {
        return _suggestionsDict[id];
    }
}
