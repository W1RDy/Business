using UnityEngine;

public class CoinsDistributeSlider : CustomSlider
{
    [SerializeField] private CoinsDistributor _coinsDistributor; 

    protected override void Init()
    {
        base.Init();
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
