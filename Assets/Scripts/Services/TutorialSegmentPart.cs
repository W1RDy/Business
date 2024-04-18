using UnityEngine;

public abstract class TutorialSegmentPart : MonoBehaviour, ITutorial
{
    [SerializeField] private TutorialSegmentType _segmentType;

    public abstract void Activate();

    public abstract void Deactivate();

    public abstract bool ConditionCompleted();
}
