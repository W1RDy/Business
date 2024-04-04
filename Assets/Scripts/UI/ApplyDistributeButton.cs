public class ApplyDistributeButton : CustomButton
{
    protected override void ClickCallback()
    {
        _buttonService.DistributeCoins();
    }
}