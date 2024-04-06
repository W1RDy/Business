using UnityEngine;

public class ProblemsGenerator : IService
{
    private ProblemWindow _problemWindow;
    private WindowActivator _windowActivator;
    private RandomController _randomController;

    public ProblemsGenerator(ProblemConfig[] problemConfigs, RandomController randomController)
    {
        _randomController = randomController;
        InitConfigsInstnces(problemConfigs);

        _problemWindow = ServiceLocator.Instance.Get<WindowService>().GetWindow(WindowType.ProblemWindow) as ProblemWindow;
        _windowActivator = ServiceLocator.Instance.Get<WindowActivator>();
    }

    private void InitConfigsInstnces(ProblemConfig[] problemConfigs)
    {
        var problemConfigsInstance = new ProblemConfig[problemConfigs.Length];

        for (int i = 0; i < problemConfigs.Length; i++)
        {
            problemConfigsInstance[i] = ScriptableObject.Instantiate(problemConfigs[i]);
        }
        _randomController.Init(problemConfigsInstance);
    }

    private ProblemConfig GetRandomProblem()
    {
        var randomizable = _randomController.GetRandomizableWithChances();
        if (randomizable is ProblemConfig problemConfig)
        {
            _randomController.BlockController();
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
            _problemWindow.InitProblem(problemConfig);

            _windowActivator.ActivateWindow(WindowType.ProblemWindow);
        }
    }
}