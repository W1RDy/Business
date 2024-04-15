using UnityEngine;

[CreateAssetMenu(fileName = "SuggestionByProblem", menuName = "Suggestions/New Suggestion By Problem")]
public class SuggestionByProblem : Suggestion
{
    private ProblemConfig _problem;

    public void SetProblem(ProblemConfig problem)
    {
        _problem = problem;
    }

    public override void Apply()
    {
        base.Apply();
        _problem.Apply();
    }
}