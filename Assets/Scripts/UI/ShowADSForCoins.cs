using UnityEngine;
using YG;

public class ShowADSForCoins : ShowADSForRewardButton
{
    [SerializeField] private float _duration;

    [SerializeField] private UIIncreaseAnimation _increaseAnimation;
    [SerializeField] private UIShrinkAnimation _shrinkAnimation;

    private UIIncreaseAnimation _increaseAnimationInstance;
    private UIShrinkAnimation _shrinkAnimationInstance;
    public float Duration => _duration;

    public override void Init()
    {
        base.Init();
        SetText("Continue");

        _increaseAnimationInstance = Instantiate(_increaseAnimation);
        _shrinkAnimationInstance = Instantiate(_shrinkAnimation);

        _increaseAnimationInstance.SetParametres(transform);
        _shrinkAnimationInstance.SetParametres(transform);
    }

    protected override void ClickCallback()
    {
        if (!_increaseAnimationInstance.IsFinished) _increaseAnimationInstance.Kill();
        if (!_shrinkAnimationInstance.IsFinished) _shrinkAnimationInstance.Kill();

        base.ClickCallback();

    }

    protected override void ActivateADS()
    {
        base.ActivateADS();
        YandexGame.RewVideoShow(0);
        HideButton();
    }

    public override void ActivateButton()
    {
        base.ActivateButton();
        _increaseAnimationInstance.Play();
    }

    public override void HideButton()
    {
        if (!_increaseAnimationInstance.IsFinished) _increaseAnimationInstance.Kill();
        _shrinkAnimationInstance.Play(() => gameObject.SetActive(false));
    }

    public void SetCoinsView(int coins)
    {
        SetText(coins.ToString());
    }

    protected override void SetText(string coins)
    {
        _buttonText.text = "+ " + coins;
    }
}
