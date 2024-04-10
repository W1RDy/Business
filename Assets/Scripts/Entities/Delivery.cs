using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Delivery : MonoBehaviour
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

    private Pool<DeliveryOrder> _pool;

    private DeliveryOrderService _orderService;
    private CompositeOrder _compositeOrder;

    private void Awake()
    {
        _deliveryConfigInstance = Instantiate(_deliveryConfig);

        _view = new DeliveryView(_costText, _timeText, _titleText, _descriptionText, _icon);
        _view.SetView(Cost, Time, _deliveryConfig.DeliveryTitle, _deliveryConfig.DeliveryDescription, _deliveryConfig.Icon);
    }

    private void Start()
    {
        _pool = ServiceLocator.Instance.Get<Pool<DeliveryOrder>>();
        _orderService = ServiceLocator.Instance.Get<DeliveryOrderService>();
        _compositeOrder = ServiceLocator.Instance.Get<CompositeOrder>();
    }

    public void AddDeliveryOrder()
    {
        var deliveryOrder = _orderService.GetOrder(ID) as DeliveryOrder;
        if (deliveryOrder == null)
        {
            deliveryOrder = _pool.Get();
            deliveryOrder.InitVariant(ID, Cost, Time, _deliveryConfigInstance.GoodsType, _deliveryConfigInstance.Icon);
            _orderService.AddOrder(deliveryOrder);

            _compositeOrder.AddOrder(deliveryOrder);
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
