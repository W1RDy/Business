using System.Collections.Generic;
using UnityEngine;

public class SubscribeController : MonoBehaviour, IService
{
    private List<ISubscribable> _subscribables = new List<ISubscribable>();

    public void AddSubscribable(ISubscribable subscribable)
    {
        _subscribables.Add(subscribable);
    }

    public void RemoveSubscribable(ISubscribable subscribable)
    {
        _subscribables.Remove(subscribable);
    }

    private void UnsubscribeAll()
    {
        foreach (var subscribable in _subscribables)
        {
            subscribable.Unsubscribe();
        }
        _subscribables.Clear();
    }

    private void OnDestroy()
    {
        UnsubscribeAll();
    }
}
