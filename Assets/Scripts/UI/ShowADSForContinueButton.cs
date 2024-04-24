using YG;

public class ShowADSForContinueButton : ShowADSForRewardButton
{
    public override void Init()
    {
        base.Init();
        SetText("Continue");
    }

    protected override void ActivateADS()
    {
        base.ActivateADS();
        YandexGame.RewVideoShow(1);
    }
}
