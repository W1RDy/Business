public class ProblemButton : WasteCoinsButton
{
    private ProblemConfig _problem;

    public void SetProblem(ProblemConfig problem)
    {
        _problem = problem;
    }

    protected override void ClickCallback()
    {
        base.ClickCallback();
        _problem.Apply();
    }
}
