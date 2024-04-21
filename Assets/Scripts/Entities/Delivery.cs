using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Delivery : ObjectForInitialization
{
    #region Values

    [SerializeField] private DeliveryConfig _deliveryConfig;
    private DeliveryConfig _deliveryConfigInstance;

    public int ID => (int)_deliveryConfigInstance.DeliveryType;

    public int Cost => _deliveryConfigInstance.DeliveryCost;

    public int Time => _deliveryConfigInstance.DeliveryTime;

    #endregion

    #region View

    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private TextMeshProUGUI _timeText;

    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _descriptionText;

    [SerializeField] private Image _icon;

    private DeliveryView _view;

    #endregion

    private DeliveryOrderService _orderService;
    private CompositeOrder _compositeOrder;

    private DeliveryOrderGenerator _orderGenerator;
    private WindowChildChangedHandler _windowChildChangedHandler;


    public override void Init()
    {
        base.Init();
        _deliveryConfigInstance = Instantiate(_deliveryConfig);

        _view = new DeliveryView(_costText, _timeText, _titleText, _descriptionText, _icon);
        _view.SetView(Cost, Time, _deliveryConfig.DeliveryTitle, _deliveryConfig.DeliveryDescription, _deliveryConfig.Icon);

        _orderGenerator = ServiceLocator.Instance.Get<DeliveryOrderGenerator>();
        _orderService = ServiceLocator.Instance.Get<DeliveryOrderService>();
        _compositeOrder = ServiceLocator.Instance.Get<CompositeOrder>();

        var windowService = ServiceLocator.Instance.Get<WindowService>();
        _windowChildChangedHandler = new WindowChildChangedHandler(windowService.GetWindow(WindowType.BasketWindow));
    }

    public void AddDeliveryOrder()
    {
        Action action = () => AddOrderDelegate();
        _windowChildChangedHandler.ChangeChilds(action);
    }

    private void AddOrderDelegate()
    {
        var deliveryOrder = _orderService.GetOrder(ID) as DeliveryOrder;
        if (deliveryOrder == null)
        {
            _orderGenerator.GenerateOrder(ID, Cost, Time, _deliveryConfigInstance.GoodsType, _deliveryConfigInstance.Icon);
        }
        else
        {
            var oldCost = deliveryOrder.Cost;
            var oldTime = deliveryOrder.Time;

            deliveryOrder.Amount++;

            _compositeOrder.ChangeOrder(oldCost, oldTime, deliveryOrder);
        }
    }
}
