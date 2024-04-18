using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;

public class TutorialService
{
    private Dictionary<TutorialSegmentType, TutorialSegment> _tutorialSegments = new Dictionary<TutorialSegmentType, TutorialSegment>();

    public TutorialService(TutorialSegment[] segments)
    {
        InitDictionary(segments);
    }

    private void InitDictionary(TutorialSegment[] segments)
    {
        foreach (TutorialSegment segment in segments)
        {
            _tutorialSegments.Add(segment.SegmentType, segment);
        }
    }

    public TutorialSegment GetTutorialSegment(TutorialSegmentType segmentType)
    {
        return _tutorialSegments[segmentType];
    }
}

public enum TutorialSegmentType
{
    CompleteOrder,
    DistributeCoins
}
