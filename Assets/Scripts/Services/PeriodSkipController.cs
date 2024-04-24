using UnityEngine;
using UnityEngine.UI;

public class PeriodSkipController : ObjectForInitialization, IService
{
    private GamesConditionChecker _conditionChecker;
    private AudioPlayer _audioPlayer;

    private ProblemsGenerator _problemsGenerator;
    private OrderGenerator _orderGenerator;
    private OrderUrgencyUpdater _orderUrgencyUpdater;
    private DarknessAnimationController _animationController;

    private DataSaver _dataSaver;

    public override void Init()
    {
        base.Init();
        _conditionChecker = ServiceLocator.Instance.Get<GamesConditionChecker>();

        _animationController = ServiceLocator.Instance.Get<DarknessAnimationController>();

        _audioPlayer = ServiceLocator.Instance.Get<AudioPlayer>();
        _problemsGenerator = ServiceLocator.Instance.Get<ProblemsGenerator>();
        _orderGenerator = ServiceLocator.Instance.Get<OrderGenerator>();
        _orderUrgencyUpdater = new OrderUrgencyUpdater();

        _dataSaver = ServiceLocator.Instance.Get<DataSaver>();
    }

    public void SkipDays()
    {
        _animationController.PlayDarknessAnimation(() => 
        {
            if (!_conditionChecker.IsPeriodFinished() && !_conditionChecker.IsGameFinished()) ContinueNewDay(); 
        });

        if (_conditionChecker.IsPeriodFinished())
        {
            _dataSaver.StartSavingWithView();
        }
        _audioPlayer.PlaySound("SkipTime");
    }

    public void SkipDaysWithoutSaving()
    {
        _animationController.PlayDarknessAnimation(() =>
        {
            if (!_conditionChecker.IsPeriodFinished() && !_conditionChecker.IsGameFinished()) ContinueNewDay();
        });
        _audioPlayer.PlaySound("SkipTime");
    }

    public void ContinueNewDay()
    {
        _orderUrgencyUpdater.UpdateUrgency();
        _problemsGenerator.TryGenerateProblem();
        _orderGenerator.ActivateOrderGenerator();
        _animationController.PlayBrightnessAnimation(null);
    }
}