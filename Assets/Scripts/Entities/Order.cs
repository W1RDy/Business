using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OrderView))]
public class Order : MonoBehaviour, IOrder, IPoolElement<Order>
{
    #region Values

    [SerializeField] private OrderConfig _orderConfig;

    private int _id;
    private int _remainTime;

    public int ID => _id;
    public int Cost => _orderConfig.Cost;
    public int Time => _orderConfig.Time;

    #endregion

    private bool _isApplied;
    public bool IsApplied => _isApplied;

    private OrderView _view;

    private RewardHandler _rewardHandler;
    private TimeController _timeController;
    private ActiveOrderService _activeOrderService;
    private OrderService _orderService;

    private Action<int> TimeChangedDelegate;

    private Goal _goal;
    private Pool<Goal> _pool;

    private bool _isInitialized;
    private bool _isFree;
    public bool IsFree => _isFree;
    public Order Element => this;


    public void Init(int id, OrderConfig orderConfig)
    {
        if (!_isInitialized) InitDependency();

        _orderConfig = orderConfig;
        _id = id;

        _view.SetView(_orderConfig.Cost, _orderConfig.Time);

        _remainTime = _orderConfig.Time;
    }

    private void InitDependency()
    {
        _isInitialized = true;

        _view = GetComponent<OrderView>();

        _rewardHandler = ServiceLocator.Instance.Get<RewardHandler>();
        _timeController = ServiceLocator.Instance.Get<TimeController>();
        _activeOrderService = ServiceLocator.Instance.Get<ActiveOrderService>();
        _orderService = ServiceLocator.Instance.Get<OrderService>();

        TimeChangedDelegate = changedValue => ChangeRemainTime(changedValue);

        _timeController.OnTimeChanged += TimeChangedDelegate;

        _pool = ServiceLocator.Instance.Get<Pool<Goal>>();
    }

    public void ApplyOrder()
    {
        if (!_isApplied)
        {
            Debug.Log("Order applied");
            _isApplied = true;

            _goal = _pool.Get();
            _goal.Init(ID, Cost, Time);
        }
    }

    public void CancelOrder()
    {
        if (_isApplied)
        {
            Debug.Log("Order canceled");
            _isApplied = false;

            _activeOrderService.RemoveOrder(this);
            _orderService.RemoveOrder(this);
        }
    }

    public void CompleteOrder()
    {
        if (_isApplied)
        {
            Debug.Log("Order completed");
            _isApplied = false;
            _rewardHandler.ApplyRewardForOrder(this);

            _activeOrderService.RemoveOrder(this);
            _orderService.RemoveOrder(this);
        }
    }

    private void ChangeRemainTime(int changeValue)
    {
        _remainTime = Mathf.Clamp(_remainTime - changeValue, 0, _orderConfig.Time);

        if (_remainTime == 0)
        {
            CancelOrder();
            _goal.DestroyGoal();
        }
        else
        {
            _goal.SetRemainTime(_remainTime);
        }
    }

    public void OnDestroy()
    {
        if (_timeController != null)
        {
            _timeController.OnTimeChanged -= TimeChangedDelegate;
        }
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        _isFree = false;
    }

    public void Release()
    {
        gameObject.SetActive(false);
        _isFree = true;
    }
}
