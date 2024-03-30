using System.Collections.Generic;

public class GoalPool : IPool<Goal>
{
    private int _poolStartSize;

    private List<IPoolElement<Goal>> _goalList = new List<IPoolElement<Goal>>();

    private GoalFactory _factory;

    public GoalPool(GoalFactory factory, int startSize)
    {
        _factory = factory;
        _poolStartSize = startSize;
    }

    public void Init()
    {
        for (int i = 0; i < _poolStartSize; i++)
        {
            var goal = Create();

            if (goal as IPoolElement<Goal> == null) throw new System.ArgumentException("Goal doesn't realize IPoolElement interface!");

            goal.Release();
        }
    }

    public Goal Create()
    {
        if (_goalList.Count == _poolStartSize) _poolStartSize++;

        var goal = _factory.Create() as Goal;
        _goalList.Add(goal);

        return goal;
    }

    public Goal Get()
    {
        foreach (var goal in _goalList)
        {
            if (goal.IsFree)
            {
                goal.Activate();
                return goal.Element;
            }
        }
        return Create();
    }

    public void Release(Goal goal)
    {
        goal.Release();
    }
}
