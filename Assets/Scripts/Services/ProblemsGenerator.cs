using System;
using UnityEngine;

public class ProblemsGenerator : ClassForInitialization, IService, ISubscribable
{
    private ProblemConfig[] _problemConfigsInstance;

    private ProblemWindow _problemWindow;
    private WindowActivator _windowActivator;
    private RandomController _randomController;
    private ClicksBlocker _clicksBlocker;

    private RememberedOrderService _rememberedOrderService;

    private DifficultyController _difficultyController;
    private Action _onDifficultyChanged;

    private SubscribeController _subscribeController;

    public ProblemsGenerator(ProblemConfig[] problemConfigs, RandomController randomController) : base()
    {
        _randomController = randomController;
        InitConfigsInstances(problemConfigs);
    }

    public override void Init()
    {
        _problemWindow = ServiceLocator.Instance.Get<WindowService>().GetWindow(WindowType.ProblemWindow) as ProblemWindow;
        _windowActivator = ServiceLocator.Instance.Get<WindowActivator>();
        _rememberedOrderService = ServiceLocator.Instance.Get<RememberedOrderService>();
        _difficultyController = ServiceLocator.Instance.Get<DifficultyController>();
        _subscribeController = ServiceLocator.Instance.Get<SubscribeController>();
        _clicksBlocker = ServiceLocator.Instance.Get<ClicksBlocker>();

        Subscribe();
    }

    private void InitConfigsInstances(ProblemConfig[] problemConfigs)
    {
        _problemConfigsInstance = new ProblemConfig[problemConfigs.Length];

        for (int i = 0; i < problemConfigs.Length; i++)
        {
            _problemConfigsInstance[i] = ScriptableObject.Instantiate(problemConfigs[i]);
        }
        _randomController.Init(_problemConfigsInstance);
    }

    private ProblemConfig GetRandomProblem()
    {
        var randomizable = _randomController.GetRandomizableWithChances();
        if (randomizable is ProblemConfig problemConfig)
        {
            _randomController.BlockController();

            problemConfig.InitProblemValues();

            if (problemConfig is ProblemWithOrder problemWithOrder)
            {
                if (_rememberedOrderService.GetOrdersCount() == 0) return null;
                problemWithOrder.SetParameters(_rememberedOrderService.PopOrder());
                _rememberedOrderService.ClearLastOrdersExcept(0);
            }
            else _rememberedOrderService.ClearLastOrdersExcept(3);

            return problemConfig;
        }

        if (_randomController.BlockedCounts >= _randomController.MinBlockedCounts) _randomController.UnblockController();
        return null;
    }

    public void TryGenerateProblem()
    {
        var problemConfig = GetRandomProblem();
        if (problemConfig != null)
        {
            _problemWindow.SetProblem(problemConfig);
            _clicksBlocker.BlockClicks();
            _windowActivator.ActivateWindow(WindowType.ProblemWindow);
        }
    }

    private void ChangeGenerateChancesByDifficulty()
    {
        _randomController.ChangeChances(_difficultyController.ProblemChances);
    }

    private void ChangeProblemsCostByDifficulty()
    {
        foreach (var problemConfigInstance in _problemConfigsInstance)
        {
            problemConfigInstance.CoinsDifficultyValue = _difficultyController.ProblemCostValue;
        }
    }

    private void ChangeMinSkipsBetweenProblemsByDifficulty()
    {
        _randomController.ChangeMinBlocksCount(_difficultyController.MinSkipsBetweenProblems);
    }

    public void Subscribe()
    {
        _subscribeController.AddSubscribable(this);

        _onDifficultyChanged = () =>
        {
            ChangeGenerateChancesByDifficulty();
            ChangeProblemsCostByDifficulty();
            ChangeMinSkipsBetweenProblemsByDifficulty();
        };

        _difficultyController.DifficultyChanged += _onDifficultyChanged;
        _onDifficultyChanged.Invoke();
    }

    public void Unsubscribe()
    {
        _difficultyController.DifficultyChanged -= _onDifficultyChanged;
    }
}
