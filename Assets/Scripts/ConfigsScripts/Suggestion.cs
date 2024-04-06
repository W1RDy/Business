using System;
using UnityEngine;

public abstract class Suggestion : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private string _text;
    public string Text => _text;
    public string Id => _id;

    public event Action SuggestionApplied;
    public event Action SuggestionSkipped;

    public virtual void ApplySuggestion()
    {
        SuggestionApplied?.Invoke();
    }

    public virtual void SkipSuggestion()
    {
        SuggestionSkipped?.Invoke();
    }
}
