using UnityEngine;

public class ProblemsGenerator : IService
{
    private ProblemConfig[] _problemConfigs;
    private ProblemWindow _problemWindow;

    private WindowActivator _windowActivator;

    public ProblemsGenerator(ProblemConfig[] problemConfigs)
    {
        InitConfigsInstnces(problemConfigs);

        _problemWindow = ServiceLocator.Instance.Get<WindowService>().GetWindow(WindowType.ProblemWindow) as ProblemWindow;
        _windowActivator = ServiceLocator.Instance.Get<WindowActivator>();
    }

    private void InitConfigsInstnces(ProblemConfig[] problemConfigs)
    {
        _problemConfigs = new ProblemConfig[problemConfigs.Length];

        for (int i = 0; i < problemConfigs.Length; i++)
        {
            _problemConfigs[i] = ScriptableObject.Instantiate(problemConfigs[i]);
        }
    }

    private ProblemConfig GetRandomProblem()
    {
        return RandomizerWithChances<ProblemConfig>.Randomize(_problemConfigs);
    }

    public void GenerateProblem()
    {
        var problemConfig = GetRandomProblem();
        _problemWindow.InitProblem(problemConfig);

        _windowActivator.ActivateWindow(WindowType.ProblemWindow);
    }
}