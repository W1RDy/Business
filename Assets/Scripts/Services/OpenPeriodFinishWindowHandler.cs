public class OpenPeriodFinishWindowHandler : ClassForInitialization
{
    private TutorialActivator _tutorialActivator;
    private WindowActivator _windowActivator;
    private bool _isTutorial = true;

    public OpenPeriodFinishWindowHandler() : base() { }

    public override void Init()
    {
        _tutorialActivator = ServiceLocator.Instance.Get<TutorialActivator>();
        _windowActivator = ServiceLocator.Instance.Get<WindowActivator>();
    }

    public void OpenFinishPeriodWindow()
    {
        if (_isTutorial)
        {
            _tutorialActivator.ActivateTutorial(TutorialSegmentType.DistributeCoins);
            _isTutorial = false;
        }
        _windowActivator.ActivateWindow(WindowType.FinishPeriodWindow);
    }
}