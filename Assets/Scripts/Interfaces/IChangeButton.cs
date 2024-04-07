public interface IChangeButton
{
    public CustomButton ButtonForChange { get; }

    public bool CheckChangeCondition();
}