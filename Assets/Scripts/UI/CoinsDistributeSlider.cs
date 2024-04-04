public class CoinsDistributeSlider : CustomSlider
{
    private CoinsDistributor _coinsDistributor;

    protected override void Init()
    {
        base.Init();
        _coinsDistributor = ServiceLocator.Instance.Get<CoinsDistributor>();
    }

    protected override void OnValueChangedCallback(float value)
    {
        if (_oldValue != value)
        {
            _coinsDistributor.DistributeMoney(value);
            base.OnValueChangedCallback(value);
        }
    }
}
