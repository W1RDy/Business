using System;
using UnityEngine;

public class ProblemsGenerator : IService
{
    private ProblemWindow _problemWindow;
    private WindowActivator _windowActivator;
    private RandomController _randomController;

    private RememberedOrderService _rememberedOrderService;
    private Action InitDelegate;

    public ProblemsGenerator(ProblemConfig[] problemConfigs, RandomController randomController)
    {
        InitDelegate = () =>
        {
            _randomController = randomController;
            InitConfigsInstances(problemConfigs);

            _problemWindow = ServiceLocator.Instance.Get<WindowService>().GetWindow(WindowType.ProblemWindow) as ProblemWindow;
            _windowActivator = ServiceLocator.Instance.Get<WindowActivator>();
            _rememberedOrderService = ServiceLocator.Instance.Get<RememberedOrderService>();
            Debug.Log(_rememberedOrderService);

            ServiceLocator.Instance.ServiceRegistered -= InitDelegate;
        };
        ServiceLocator.Instance.ServiceRegistered += InitDelegate;
        if (ServiceLocator.Instance.IsRegistered) InitDelegate.Invoke();
    }

    private void InitConfigsInstances(ProblemConfig[] problemConfigs)
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

            _windowActivator.ActivateWindow(WindowType.ProblemWindow);
        }
    }
}
