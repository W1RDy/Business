using System;
using YG;

public class TutorialActivator : IService
{
    public event Action TutorialActivated;
    public event Action TutorialDeactivated;

    private TutorialService _service;
    private TutorialController _controller;

    private TutorialSegment _currentSegment;
    private DataSaver _dataSaver;

    public TutorialActivator(TutorialService tutorialService, TutorialController tutorialController)
    {
        _service = tutorialService;
        _controller = tutorialController;

        _dataSaver = ServiceLocator.Instance.Get<DataSaver>();
    }

    public void ActivateTutorial(TutorialSegmentType segmentType)
    {
        TutorialActivated?.Invoke();

        _currentSegment = _service.GetTutorialSegment(segmentType);
        _currentSegment.Activate();
        _controller.SetSegment(_currentSegment);
    }

    public void DeactivateTutorial()
    {
        _currentSegment.Deactivate();
        _dataSaver.SaveTutorialState();

        TutorialDeactivated?.Invoke();
    }
}
