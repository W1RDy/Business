using System;
using System.Collections.Generic;

public class WindowChildChangedHandler : ISubscribable
{
    private Window _window;
    private SubscribeController _subscribeController;
    private Queue<Action> _actions = new Queue<Action>();

    public int ActionCount => _actions.Count;

    public WindowChildChangedHandler(Window window)
    {
        _window = window;
        _subscribeController = ServiceLocator.Instance.Get<SubscribeController>();

        _subscribeController.AddSubscribable(this);
    }

    public void ChangeChilds(Action changeAction)
    {
        if (_window.IsChanging)
        {
            Action action = () => { };
            action = () =>
            {
                changeAction.Invoke();
                _actions.Dequeue();
                UnsubscribeAction(action);
            };

            SubscribeAction(action);
            _actions.Enqueue(action);
        }
        else changeAction.Invoke();
    }

    public void Subscribe()
    {
    }

    public void Unsubscribe()
    {
        while (_actions.Count > 0)
        {
            var action = _actions.Dequeue();
            UnsubscribeAction(action);
        }
    }

    private void SubscribeAction(Action action)
    {
        _window.OnWindowChanged += action;
    }

    private void UnsubscribeAction(Action action)
    {
        _window.OnWindowChanged -= action;
    }
}
