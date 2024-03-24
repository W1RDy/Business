using CoinsCounter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocatorLoader : MonoBehaviour
{
    [SerializeField] private TimeIndicator _timeIndicator;
    [SerializeField] private CoinsIndicator _handsCoinsIndicator;
    [SerializeField] private CoinsIndicator _bankCoinsIndicator;

    private void Awake()
    {
        Bind();
    }

    private void Bind()
    {
        BindTimeController();
        BindCoinsCounters();
        BindButtonService();
    }

    private void BindButtonService()
    {
        var buttonService = new ButtonService();
        ServiceLocator.Instance.Register(buttonService);
    }

    private void BindTimeController()
    {
        var timeController = new TimeController(_timeIndicator);
        ServiceLocator.Instance.Register(timeController);
    }

    private void BindCoinsCounters()
    {
        var bankCoinsCounter = new BankCoinsCounter(_bankCoinsIndicator);
        var handCoinsCounter = new HandsCoinsCounter(_handsCoinsIndicator);

        ServiceLocator.Instance.Register(bankCoinsCounter);
        ServiceLocator.Instance.Register(handCoinsCounter);
    }
}
