using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalPool : IPool<Goal>, IService
{
    private int _poolStartSize;

    private List<IPoolElement<Goal>> _goalList = new List<IPoolElement<Goal>>();

    private GoalFactory _factory;
    private Transform _parent;
    private RectTransform _poolContainer;

    public GoalPool(RectTransform poolContainer, Transform parent, int startSize)
    {
        _poolContainer = poolContainer;
        _parent = parent;

        _factory = new GoalFactory(poolContainer);
        _factory.LoadResources();

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
                goal.Element.transform.SetParent(_parent);
                goal.Activate();
                return goal.Element;
            }
        }

        var newGoal = Create();
        newGoal.transform.SetParent(_parent);

        return newGoal;
    }

    public void Release(Goal goal)
    {
        goal.Release();
        goal.transform.SetParent(_poolContainer);
    }
}
