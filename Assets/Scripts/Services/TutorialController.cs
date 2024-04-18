using System;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialController : ObjectForInitialization
{
    private TutorialActivator _tutorialActivator;

    private TutorialSegment _currentSegment;

    private OrderGenerator _orderGenerator;

    public override void Init()
    {
        base.Init();
        _tutorialActivator = ServiceLocator.Instance.Get<TutorialActivator>();
        _orderGenerator = ServiceLocator.Instance.Get<OrderGenerator>();
    }

    public void SetSegment(TutorialSegment segment)
    {
        _currentSegment = segment;
        if (segment.SegmentType == TutorialSegmentType.CompleteOrder) _orderGenerator.GenerateTutorialOrder();
    }

    private void Update()
    {
        if (_currentSegment != null)
        {
            if (_currentSegment.ConditionCompleted())
            {
                if (_currentSegment.IsNextPartEmpty())
                {
                    _currentSegment = null;
                    _tutorialActivator.DeactivateTutorial();
                }
                else _currentSegment.ActivateNextPart();
            }
        }
    }
}