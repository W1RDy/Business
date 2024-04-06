using System;
using UnityEngine;

public abstract class Suggestion : ScriptableObject, IEvent
{
    [SerializeField] private string _id;
    [SerializeField] private string _description;
    public string Description => _description;
    public string ID => _id;

    public event Action Applied;
    public event Action Skipped;

    public virtual void Apply()
    {
        Applied?.Invoke();
    }

    public virtual void Skip()
    {
        Skipped?.Invoke();
    }
}
