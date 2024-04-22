using System;
using UnityEngine;
using UnityEngine.UI;

public class PeriodSkipController : ObjectForInitialization, IService
{
    [SerializeField] private UIFadeAnimationWithText _darknessAnimation;
    [SerializeField] private UIFadeAnimationWithText _brightnessAnimation;
    private UIFadeAnimationWithText _darknessAnimationInstance;
    private UIFadeAnimationWithText _brightnessAnimationInstance;

    [SerializeField] private CustomImage _darknessView;

    [SerializeField] private ClicksBlocker _clicksBlocker;

    private GamesConditionChecker _conditionChecker;
    private AudioPlayer _audioPlayer;

    private ProblemsGenerator _problemsGenerator;
    private OrderGenerator _orderGenerator;
    private OrderUrgencyUpdater _orderUrgencyUpdater;

    private DataSaver _dataSaver;

    public override void Init()
    {
        base.Init();
        _conditionChecker = ServiceLocator.Instance.Get<GamesConditionChecker>();

        _darknessAnimationInstance = Instantiate(_darknessAnimation);
        _brightnessAnimationInstance = Instantiate(_brightnessAnimation);

        _darknessAnimationInstance.SetParameters(_darknessView);
        _brightnessAnimationInstance.SetParameters(_darknessView);

        _audioPlayer = ServiceLocator.Instance.Get<AudioPlayer>();
        _problemsGenerator = ServiceLocator.Instance.Get<ProblemsGenerator>();
        _orderGenerator = ServiceLocator.Instance.Get<OrderGenerator>();
        _orderUrgencyUpdater = new OrderUrgencyUpdater();

        _dataSaver = ServiceLocator.Instance.Get<DataSaver>();
    }

    public void SkipDays()
    {
        _clicksBlocker.BlockClicks();
        _darknessAnimationInstance.Play(() => 
        {
            if (!_conditionChecker.IsPeriodFinished() && !_conditionChecker.IsGameFinished()) ContinueNewDay(); 
        });
        if (_conditionChecker.IsPeriodFinished())
        {
            _dataSaver.SaveTutorialState();
            _dataSaver.StartSaving();
        }
        _audioPlayer.PlaySound("SkipTime");
    }

    public void ContinueNewDay()
    {
        _orderUrgencyUpdater.UpdateUrgency();
        _problemsGenerator.TryGenerateProblem();
        _orderGenerator.ActivateOrderGenerator();
        _clicksBlocker.UnblockClicks();
        _brightnessAnimationInstance.Play();
    }
}
