using System;
using UnityEngine;
using UnityEngine.UI;

public class PeriodSkipController : MonoBehaviour, IService
{
    [SerializeField] private UIFadeAnimationWithText _darknessAnimation;
    [SerializeField] private UIFadeAnimationWithText _brightnessAnimation;
    private UIFadeAnimationWithText _darknessAnimationInstance;
    private UIFadeAnimationWithText _brightnessAnimationInstance;

    [SerializeField] private CustomImage _darknessView;

    [SerializeField] private ClicksBlocker _clicksBlocker;

    private GamesConditionChecker _conditionChecker;

    private Action InitDelegate;

    private AudioPlayer _audioPlayer;

    private ProblemsGenerator _problemsGenerator;

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
            _problemsGenerator = ServiceLocator.Instance.Get<ProblemsGenerator>();

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
        _problemsGenerator.TryGenerateProblem();
        _clicksBlocker.UnblockClicks();
        _brightnessAnimationInstance.Play();
    }
}
