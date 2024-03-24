public class CloseButton : CustomButton
{
    protected override void ClickCallback()
    {
        base.ClickCallback();
        _buttonService.ClosePeriodWindow();
    }
}