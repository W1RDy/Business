using System;
using UnityEngine;

public class ProblemsGenerator : ClassForInitialization, IService, ISubscribable
{
    private ProblemConfig[] _problemConfigsInstance;

    private ProblemWindow _problemWindow;
    private WindowActivator _windowActivator;
    private RandomController _randomController;
    private ClicksBlocker _clicksBlocker;
    private GameController _gameController;

    private RememberedOrderService _rememberedOrderService;

    private DifficultyController _difficultyController;
    private Action _onDifficultyChanged;

    private SubscribeController _subscribeController;
    //private DataSaver _dataSaver;

    private ProblemConfig _currentProblem;

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
        _gameController = ServiceLocator.Instance.Get<GameController>();
        _clicksBlocker = ServiceLocator.Instance.Get<ClicksBlocker>();

        //_dataSaver = ServiceLocator.Instance.Get<DataSaver>();

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
        Debug.Log("TryGenerate");
        if (_gameController.IsTutorial) return;

        _currentProblem = GetRandomProblem();
        if (_currentProblem != null)
        {
            _problemWindow.SetProblem(_currentProblem);
            _clicksBlocker.BlockClicks();
            _windowActivator.ActivateWindow(WindowType.ProblemWindow);

            //SaveDelegate();
        }
    }

    //private void SaveDelegate()
    //{
    //    _dataSaver.SaveProblem(_currentProblem);
    //    _currentProblem = null;
    //}

    //public void GenerateProblemByLoadData(ProblemSaveConfig problem)
    //{
    //    _randomController.BlockController();

    //    var problemConfig = GetProblem(problem.id);
    //    if (problemConfig is ProblemWithOrder problemWithOrder) problemWithOrder.SetParameters(problem.problemedOrder);

    //    _problemWindow.SetProblem(problemConfig);
    //    _clicksBlocker.BlockClicks();
    //    _windowActivator.ActivateWindow(WindowType.ProblemWindow);
    //}

    private ProblemConfig GetProblem(string index)
    {
        foreach (var problem in _problemConfigsInstance)
        {
            if (problem.ID == index) return problem;
        }
        throw new System.ArgumentNullException("Problem with index " + index + " doesn't exist!");
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

        //_dataSaver.OnStartSaving += SaveDelegate;
    }

    public void Unsubscribe()
    {
        _difficultyController.DifficultyChanged -= _onDifficultyChanged;
        //_dataSaver.OnStartSaving -= SaveDelegate;
    }
}
