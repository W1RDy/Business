using TMPro;
using UnityEngine;

public class DeliveryOrder : MonoBehaviour, IOrder, IPoolElement<DeliveryOrder>
{
    #region Values

    public int ID { get; private set; }

    public int Cost { get; private set; }

    public int Time { get; private set; }

    private int _amount;

    public int Amount 
    {
        get => _amount;
        set 
        {
            Cost = value * (Cost / Mathf.Clamp(_amount, 1, int.MaxValue));
            _amount = value;

            _deliveryOrderView.SetView(Cost, value);
        }
    }

    private GoodsType _goodsType;
    public GoodsType NeededGoods => _goodsType;

    #endregion

    #region View

    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private TextMeshProUGUI _amountText;

    private DeliveryOrderView _deliveryOrderView;

    #endregion

    public bool IsApplied { get; private set; }
    public bool IsFree {get; private set;}
    public DeliveryOrder Element => this;
    private CompositeOrder _compositeOrder;

    private DeliveryOrderService _deliveryOrderService;

    private GoodsGenerator _goodsGenerator;

    private Pool<DeliveryOrder> _pool;

    private void Awake()
    {
        _deliveryOrderService = ServiceLocator.Instance.Get<DeliveryOrderService>();
        _deliveryOrderView = new DeliveryOrderView(_priceText, _amountText);
    }

    private void Start()
    {
        _pool = ServiceLocator.Instance.Get<Pool<DeliveryOrder>>();
        _compositeOrder = ServiceLocator.Instance.Get<CompositeOrder>();
    }

    public void Init(int id, int cost, int time, GoodsType goodsType)
    {
        if (_goodsGenerator == null) _goodsGenerator = ServiceLocator.Instance.Get<GoodsGenerator>();

        ID = id;
        Cost = cost;
        Time = time;
        _goodsType = goodsType;

        Amount = 1;
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
    }

    public void Release()
    {
        IsFree = true;
        IsApplied = false;
        gameObject.SetActive(false);
        if (_goodsGenerator != null) _deliveryOrderService.RemoveOrder(this);
    }
}

public class DeliveryOrderView
{
    private TextMeshProUGUI _costText;
    private TextMeshProUGUI _amountText;

    public DeliveryOrderView(TextMeshProUGUI priceText, TextMeshProUGUI amountText)
    {
        _costText = priceText;
        _amountText = amountText;
    }

    public void SetView(int cost, int amount)
    {
        _costText.text = "Cost: " + cost.ToString();
        _amountText.text = "X" + amount.ToString();
    }
}
