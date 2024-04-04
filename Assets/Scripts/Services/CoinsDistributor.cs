using CoinsCounter;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics.Contracts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinsDistributor : MonoBehaviour, IService
{
    #region View

    [SerializeField] private Slider _slider;

    [SerializeField] private TextMeshProUGUI _handsCoinsText;
    [SerializeField] private TextMeshProUGUI _bankCoinsText;

    private CoinsDistributorView _view;

    #endregion 

    private int _handCoins;
    private int _bankCoins;

    private HandsCoinsCounter _handsCoinsCounter;
    private BankCoinsCounter _bankCoinsCounter;

    private void Awake()
    {
        _handsCoinsCounter = ServiceLocator.Instance.Get<HandsCoinsCounter>();
        _bankCoinsCounter = ServiceLocator.Instance.Get<BankCoinsCounter>();

        _view = new CoinsDistributorView(_slider, _handsCoinsText, _bankCoinsText);
    }

    private void OnEnable()
    {
        _handCoins = _handsCoinsCounter.Coins;
        _bankCoins = _bankCoinsCounter.Coins;

        _view.ChangeView(_handCoins, _bankCoins, (float)_bankCoins / (_handCoins + _bankCoins));
    }

    public void DistributeMoney(float distributeValue)
    {
        var sumCoins = _handsCoinsCounter.Coins + _bankCoinsCounter.Coins;

        _bankCoins = RoundToDividedFiveNumber((int)(distributeValue * sumCoins));
        _handCoins = sumCoins - _bankCoins;

        _view.ChangeView(_handCoins, _bankCoins, distributeValue);
    }

    public void ApplyDistributing()
    {
        _handsCoinsCounter.ChangeCoins(_handCoins);
        _bankCoinsCounter.ChangeCoins(_bankCoins);
    }

    private int RoundToDividedFiveNumber(int number)
    {
        var remains = number % 5;
        if (remains >= 3) return Mathf.Clamp(number + (5 - remains), 0, _handCoins + _bankCoins);
        else return Mathf.Clamp(number - remains, 0, _handCoins + _bankCoins);
    }
}

public class CoinsDistributorView
{
    private Slider _slider;

    private TextMeshProUGUI _handsCoinsText;
    private TextMeshProUGUI _bankCoinsText;

    public CoinsDistributorView(Slider slider, TextMeshProUGUI handsCoinsText, TextMeshProUGUI bankCoinsText)
    {
        _slider = slider;
        _handsCoinsText = handsCoinsText;
        _bankCoinsText = bankCoinsText;
    }

    public void ChangeView(int handsCoins, int bankCoins, float distributeValue)
    {
        _slider.value = distributeValue;

        _handsCoinsText.text = handsCoins.ToString();
        _bankCoinsText.text = bankCoins.ToString();
    }
}
