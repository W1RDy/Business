using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OrderView))]
public class Order : MonoBehaviour, IOrder, IThrowable, IPoolElement<Order>
{
    #region Values

    [SerializeField] private OrderConfig _orderConfig;

    private int _id;
    private int _remainTime;

    public int ID => _id;
    public int Cost => _orderConfig.Cost;
    public int Time => _orderConfig.Time;
    public GoodsType NeededGoods => _orderConfig.NeededGoods;

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
    private Pool<Goal> _goalPool;
    private Pool<Order> _orderPool;

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

        _goalPool = ServiceLocator.Instance.Get<Pool<Goal>>();
        _orderPool = ServiceLocator.Instance.Get<Pool<Order>>();
    }

    public void ApplyOrder()
    {
        if (!_isApplied)
        {
            Debug.Log("Order applied");
            _isApplied = true;

            _goal = _goalPool.Get();
            _goal.Init(ID, Cost, Time);

            _view.ChangeApplyState(true);
        }
    }

    public void CancelOrder()
    {
        if (_isApplied)
        {
            Debug.Log("Order canceled");
            _orderPool.Release(this);
        }
    }

    public void ChangeOrderState(bool isReadyForComplete)
    {
        if (_isApplied)
        {
            if (isReadyForComplete) _view.ChangeViewForComplete();
            else _view.ChangeViewForProcess();
        }
    }

    public void CompleteOrder()
    {
        if (_isApplied)
        {
            Debug.Log("Order completed");
            _rewardHandler.ApplyRewardForOrder(this);

            _orderPool.Release(this);
        }
    }

    private void ChangeRemainTime(int changeValue)
    {
        if (_isApplied)
        {
            _remainTime = Mathf.Clamp(_remainTime - changeValue, 0, _orderConfig.Time);

            if (_remainTime == 0)
            {
                CancelOrder();
            }
            else
            {
                _goal.SetRemainTime(_remainTime);
            }
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

        if (_isInitialized)
        {
            _orderService.RemoveOrder(this);

            if (_isApplied)
            {
                _activeOrderService.RemoveOrder(this);
                _goalPool.Release(_goal);
            }
        }

        _isFree = true;
        _isApplied = false;
    }

    public void ThrowOut()
    {
        if (_isApplied) CancelOrder();
        else _orderPool.Release(this);
    }
}
