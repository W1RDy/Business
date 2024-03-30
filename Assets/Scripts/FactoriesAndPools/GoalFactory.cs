using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GoalFactory : IFactory
{
    private const string Path = "Goal";

    private Goal _prefab;

    private RectTransform _container;

    public GoalFactory(RectTransform container)
    {
        _container = container;
    }

    public void LoadResources()
    {
        _prefab = Resources.Load<Goal>(Path);
    }

    public MonoBehaviour Create()
    {
        return Create(Vector2.zero, Quaternion.identity, _container);
    }

    public MonoBehaviour Create(Vector2 pos, Quaternion rotation, Transform parent)
    {
        var goal = MonoBehaviour.Instantiate(_prefab, pos, rotation, _container);

        goal.transform.localRotation = rotation;
        goal.transform.localPosition = Vector3.zero;

        var layoutRootForRebuild = goal.transform.GetChild(2).GetComponent<RectTransform>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutRootForRebuild);

        return goal;
    }
}
