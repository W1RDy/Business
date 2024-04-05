using UnityEngine;

public class ApplyDistributeButton : CustomButton
{
    [SerializeField] private CoinsDistributor _coinsDistributor;

    protected override void ClickCallback()
    {
        _buttonService.DistributeCoins(0, _coinsDistributor);
    }
}
