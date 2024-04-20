using System;

public class WindowChildChangedHandler : ISubscribable
{
    private Window _window;
    private SubscribeController _subscribeController;

    public int ActionCount { get; private set; }
    private Action _action;


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
            ActionCount++;
            _action = () =>
            {
                ActionCount--;
                changeAction.Invoke();
                Unsubscribe();
            };
            Subscribe();
        }
        else changeAction.Invoke();
    }

    public void Subscribe()
    {
        _window.OnWindowChanged += _action;
    }

    public void Unsubscribe()
    {
        _window.OnWindowChanged -= _action;
    }
}
