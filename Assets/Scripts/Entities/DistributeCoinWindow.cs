using UnityEngine;

public class DistributeCoinWindow : SuggestionWindow
{
    [SerializeField] private CoinsDistributor _coinsDistributor;
    [SerializeField] private ApplyDistributeWithSuggestion _applyDistributeWithSuggestion;

    public override void SetSuggestion(Suggestion suggestion)
    {
        base.SetSuggestion(suggestion);
        _applyDistributeWithSuggestion.InitializeVariant(suggestion, _coinsDistributor);
    }
}
