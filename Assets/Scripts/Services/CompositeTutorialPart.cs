using UnityEngine;

public class CompositeTutorialPart : TutorialSegmentPart
{
    [SerializeField] private TutorialSegmentPart _mainPart;
    [SerializeField] private TutorialSegmentPart _additivePart;

    public override void Activate()
    {
        _mainPart.Activate();
        _additivePart.Activate();
    }

    public override bool ConditionCompleted()
    {
        return _mainPart.ConditionCompleted();
    }

    public override void Deactivate()
    {
        _mainPart.Deactivate();
        _additivePart.Deactivate();
    }
}
