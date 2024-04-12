using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryOrder : MonoBehaviour, IOrder, IThrowable, IPoolElement<DeliveryOrder>
{
    #region Values

    public int ID { get; private set; }
    public int Cost { get; private set; }
    public int Time { get; private set; }

    private int _price;

    private int _amount;

    public int Amount 
    {
        get => _amount;
        set 
        {
            Cost = value * _price;
            _amount = value;

            _deliveryOrderView.SetView(Cost, Time, value);
        }
    }

    private GoodsType _goodsType;
    public GoodsType NeededGoods => _goodsType;

    #endregion

    #region View

    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _amountText;

    [SerializeField] private Image _icon;

    private DeliveryOrderView _deliveryOrderView;

    [SerializeField] private UIAnimation _appearAnimation;
    [SerializeField] private UIAnimation _disappearAnimation;
    private EntityAnimationsController _animController;

    #endregion

    public bool IsApplied { get; private set; }
    public bool IsFree {get; private set;}
    public DeliveryOrder Element => this;
    private CompositeOrder _compositeOrder;

    private DeliveryOrderService _deliveryOrderService;

    private GoodsGenerator _goodsGenerator;

    private Pool<DeliveryOrder> _pool;

    private Action InitDelegate;

    public void InitInstance()
    {
        InitDelegate = () =>
        {
            Release();

            _deliveryOrderService = ServiceLocator.Instance.Get<DeliveryOrderService>();
            _deliveryOrderView = new DeliveryOrderView(_priceText, _timeText, _amountText, _icon);

            _pool = ServiceLocator.Instance.Get<Pool<DeliveryOrder>>();
            _compositeOrder = ServiceLocator.Instance.Get<CompositeOrder>();

            _goodsGenerator = ServiceLocator.Instance.Get<GoodsGenerator>();

            InitAnimations();
            ServiceLocator.Instance.ServiceRegistered -= InitDelegate;
        };

        ServiceLocator.Instance.ServiceRegistered += InitDelegate;
        if (ServiceLocator.Instance.IsRegistered) InitDelegate.Invoke();
    }

    private void InitAnimations() => _animController = new EntityAnimationsController(_appearAnimation, _disappearAnimation, gameObject);

    public void InitVariant(int id, int cost, int time, GoodsType goodsType, Sprite icon)
    {
        ID = id;
        Cost = cost;
        Time = time;
        _price = Cost;

        _goodsType = goodsType;

        Amount = 1;
        _deliveryOrderView.SetIcon(icon);
    }

    public void ApplyOrder()
    {
        if (!IsApplied)
        {
            IsApplied = true;
            CompleteOrder();
        }
    }

    public void CancelOrder()
    {
        IsApplied = false;
        if (Amount == 1)
        {
            _pool.Release(this);
            _compositeOrder.RemoveOrder(this);
        }
        else
        {
            var oldCost = Cost;
            Amount--;
            _compositeOrder.ChangeOrder(oldCost, Time, this);
        }
    }

    public void CompleteOrder()
    {
        if (IsApplied)
        {
            IsApplied = false;
            _pool.Release(this); 

            _goodsGenerator.GenerateGoods(_goodsType, Amount);
            _compositeOrder.RemoveOrder(this);
        }
    }

    public void Activate()
    {
        IsFree = false;
        gameObject.SetActive(true);

        _animController.PlayAppearAnimation();
    }

    public void Release()
    {
        IsFree = true;
        IsApplied = false;
        gameObject.SetActive(false);
        if (_goodsGenerator != null) _deliveryOrderService.RemoveOrder(this);
    }

    public void ThrowOut()
    {
        Action callback = () => CancelOrder();
        _animController.PlayDisappearAnimation(callback);
    }

    private void OnDisable()
    {
        if (_animController != null) _animController.KillAnimation();
    }
}

public class DeliveryOrderView
{
    private TextMeshProUGUI _costText;
    private TextMeshProUGUI _timeText;
    private TextMeshProUGUI _amountText;
    private Image _icon;

    public DeliveryOrderView(TextMeshProUGUI priceText, TextMeshProUGUI timeText, TextMeshProUGUI amountText, Image icon)
    {
        _costText = priceText;
        _timeText = timeText;
        _amountText = amountText;
        _icon = icon;
    }

    public void SetView(int cost, int time, int amount)
    {
        _costText.text = "- " + cost.ToString();
        _timeText.text = "+ " + time.ToString();
        _amountText.text = "X" + amount.ToString();
    }

    public void SetIcon(Sprite icon)
    {
        _icon.sprite = icon;
    }
}
