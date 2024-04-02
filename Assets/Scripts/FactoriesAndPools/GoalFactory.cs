using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GoalFactory : BaseFactory
{
    private const string Path = "Goal";

    public GoalFactory(RectTransform container) : base(container)
    {

    }

    public override void LoadResources()
    {
        if (_prefab == null) _prefab = Resources.Load<Goal>(Path);
    }

    public override MonoBehaviour Create(Vector2 pos, Quaternion rotation, Transform parent)
    {
        var goal = base.Create(pos, rotation, parent);

        var layoutRootForRebuild = goal.transform.GetChild(2).GetComponent<RectTransform>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutRootForRebuild);

        return goal;
    }
}