using System;
using UnityEngine;
using UnityEngine.UI;

public class GameLifeController : MonoBehaviour, IService
{
    [SerializeField] private UIDarknessAnimation _darknessAnimation;
    [SerializeField] private UIDarknessAnimation _brightnessAnimation;
    private UIDarknessAnimation _darknessAnimationInstance;
    private UIDarknessAnimation _brightnessAnimationInstance;

    [SerializeField] private Image _darknessView;

    [SerializeField] private ClicksBlocker _clicksBlocker;

    private TimeController _timeController;
    private Action InitDelegate;
    
    private void Start()
    {
        InitDelegate = () =>
        {
            Debug.Log("Init");
            _timeController = ServiceLocator.Instance.Get<TimeController>();

            _darknessAnimationInstance = Instantiate(_darknessAnimation);
            _brightnessAnimationInstance = Instantiate(_brightnessAnimation);

            _darknessAnimationInstance.SetParameters(_darknessView);
            _brightnessAnimationInstance.SetParameters(_darknessView);

            ServiceLocator.Instance.ServiceRegistered -= InitDelegate;
        };
        ServiceLocator.Instance.ServiceRegistered += InitDelegate;
        if (ServiceLocator.Instance.IsRegistered) InitDelegate.Invoke();
    }

    public void SkipDays()
    {
        _clicksBlocker.BlockClicks();
        _darknessAnimationInstance.Play(() => { if (!_timeController.PeriodFinished()) ContinueNewDay(); });
    }

    public void ContinueNewDay()
    {
        _brightnessAnimationInstance.Play(() => _clicksBlocker.UnblockClicks());
    }

}
