using System;

public interface ITutorial
{
    public void Activate();
    public void Deactivate();
    public bool ConditionCompleted();
}

public interface ITutorialButton
{
    public TutorialButtonType Type { get; }
    public event Action OnClick;
    public void ActivateByTutorial();
    public void DeactivateByTutorial();
}