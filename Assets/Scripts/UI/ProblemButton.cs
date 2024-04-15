public class ProblemButton : WasteCoinsButton
{
    private ProblemConfig _problem;
    private ClicksBlocker _clicksBlocker;

    protected override void Init()
    {
        base.Init();
        _clicksBlocker = ServiceLocator.Instance.Get<ClicksBlocker>();
    }

    public void SetProblem(ProblemConfig problem)
    {
        _problem = problem;
    }

    protected override void ClickCallback()
    {
        base.ClickCallback();
        _clicksBlocker.UnblockClicks();
        _problem.Apply();
    }
}
