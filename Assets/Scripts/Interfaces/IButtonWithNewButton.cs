public interface IChangeButton
{
    public ChangeCondition[] ChangeConditions { get; }
}

public interface IButtonWithNewButton : IChangeButton
{
    public CustomButton ButtonForChange { get; }

    public bool CheckButtonChangeCondition();
}

public interface IButtonWithStates : IChangeButton
{
    public void ChangeStates(bool toActiveState);

    public bool CheckStatesChangeCondition();
}

public enum ChangeCondition
{
    CoinsChanged,
    InventoryChanged,
    GameFinished,
    OrderApplied
}