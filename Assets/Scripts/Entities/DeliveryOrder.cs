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
            Cost = value * Cost;
            _amount = value;

            _deliveryOrderView.SetView(Cost, value);
        }
    }

    private GoodsType _goodsType;

    #endregion

    #region View

    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private TextMeshProUGUI _amountText;

    private DeliveryOrderView _deliveryOrderView;

    #endregion

    public bool IsApplied { get; private set; }
    public bool IsFree {get; private set;}
    public DeliveryOrder Element => this;

    private DeliveryOrderService _deliveryOrderService;

    private GoodsGenerator _goodsGenerator;

    private void Awake()
    {
        _deliveryOrderService = ServiceLocator.Instance.Get<DeliveryOrderService>();

        _deliveryOrderView = new DeliveryOrderView(_priceText, _amountText);
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
        Release();
        _deliveryOrderService.RemoveOrder(this);
    }

    public void CompleteOrder()
    {
        if (IsApplied)
        {
            IsApplied = false;
            Release();
            _deliveryOrderService.RemoveOrder(this);
            _goodsGenerator.GenerateGoods(_goodsType);
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
        gameObject.SetActive(false);
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
