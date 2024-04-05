using Random = UnityEngine.Random;

public class ProblemsGenerator : IService
{
    private ProblemConfig[] _problemConfigs;
    private ProblemWindow _problemWindow;

    private WindowActivator _windowActivator;

    public ProblemsGenerator(ProblemConfig[] problemConfigs)
    {
        _problemConfigs = problemConfigs;

        _problemWindow = ServiceLocator.Instance.Get<WindowService>().GetWindow(WindowType.ProblemWindow) as ProblemWindow;
        _windowActivator = ServiceLocator.Instance.Get<WindowActivator>();
    }

    private ProblemConfig GetRandomProblem()
    {
        var randomIndex = Random.Range(0, _problemConfigs.Length);
        return _problemConfigs[randomIndex];
    }

    public void GenerateProblem()
    {
        var problemConfig = GetRandomProblem();
        _problemWindow.InitProblem(problemConfig);

        _windowActivator.ActivateWindow(WindowType.ProblemWindow);
    }
}