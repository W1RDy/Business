using System.Collections.Generic;

public class ButtonsForTutorialService : ClassForInitialization, IService, ISubscribable
{
    private GameController _gameController;
    private Dictionary<TutorialButtonType, ITutorialButton> _buttons = new Dictionary<TutorialButtonType, ITutorialButton>();

    private bool _isServiceActive = true;

    public ButtonsForTutorialService() : base() { }

    public override void Init()
    {
        _gameController = ServiceLocator.Instance.Get<GameController>();
        Subscribe();
    }

    public void AddButton(ITutorialButton tutorialButton)
    {
        if (_isServiceActive) _buttons.Add(tutorialButton.Type, tutorialButton);
    }

    public ITutorialButton GetTutorialButton(TutorialButtonType tutorialButtonType)
    {
        if (!_isServiceActive) throw new System.ArgumentException("Servise is inactive!");
        return _buttons[tutorialButtonType];
    }

    private void ClearDictionary()
    {
        _buttons.Clear();
        _isServiceActive = false;
        Unsubscribe();
    }

    public void Subscribe()
    {
        _gameController.TutorialLevelStarted += ClearDictionary;
        _gameController.GameStarted += ClearDictionary;
    }

    public void Unsubscribe()
    {
        _gameController.TutorialLevelStarted -= ClearDictionary;
        _gameController.GameStarted -= ClearDictionary;
    }
}

public enum TutorialButtonType
{
    OpenOrders,
    ApplyOrder,
    OpenDelivery,
    OpenBasket,
    ApplyDelivery,
    ConstructPC,
    SendOrder,
    Confirm,
    OrderDelivery
}