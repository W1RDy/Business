using System;
using UnityEngine;
using UnityEngine.UI;

public class PeriodSkipController : MonoBehaviour, IService
{
    [SerializeField] private UIDarknessAnimation _darknessAnimation;
    [SerializeField] private UIDarknessAnimation _brightnessAnimation;
    private UIDarknessAnimation _darknessAnimationInstance;
    private UIDarknessAnimation _brightnessAnimationInstance;

    [SerializeField] private Image _darknessView;

    [SerializeField] private ClicksBlocker _clicksBlocker;

    private GamesConditionChecker _conditionChecker;

    private Action InitDelegate;

    private AudioPlayer _audioPlayer;
    
    private void Start()
    {
        InitDelegate = () =>
        {
            _conditionChecker = ServiceLocator.Instance.Get<GamesConditionChecker>();
            
            _darknessAnimationInstance = Instantiate(_darknessAnimation);
            _brightnessAnimationInstance = Instantiate(_brightnessAnimation);

            _darknessAnimationInstance.SetParameters(_darknessView);
            _brightnessAnimationInstance.SetParameters(_darknessView);

            _audioPlayer = ServiceLocator.Instance.Get<AudioPlayer>();

            ServiceLocator.Instance.ServiceRegistered -= InitDelegate;
        };
        ServiceLocator.Instance.ServiceRegistered += InitDelegate;
        if (ServiceLocator.Instance.IsRegistered) InitDelegate.Invoke();
    }

    public void SkipDays()
    {
        _clicksBlocker.BlockClicks();
        _darknessAnimationInstance.Play(() => 
        {
            if (!_conditionChecker.IsPeriodFinished() && !_conditionChecker.IsGameFinished()) ContinueNewDay(); 
        });
        _audioPlayer.PlaySound("SkipTime");
    }

    public void ContinueNewDay()
    {
        _brightnessAnimationInstance.Play(() => _clicksBlocker.UnblockClicks());
    }
}
