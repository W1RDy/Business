using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OrderView))]
public class Order : MonoBehaviour, IOrder, IPoolElement<Order>
{
    #region Values

    [SerializeField] private OrderConfig _orderConfig;

    private int _remainTime;

    public int ID => _orderConfig.ID;
    public int Cost => _orderConfig.Cost;
    public int Time => _orderConfig.Time;

    #endregion

    private bool _isApplied;
    public bool IsApplied => _isApplied;

    private OrderView _view;

    private RewardHandler _rewardHandler;
    private TimeController _timeController;
    private ActiveOrderService _activeOrderService;

    private Action<int> TimeChangedDelegate;

    private Goal _goal;
    private GoalPool _goalPool;

    public bool IsFree => !gameObject.activeInHierarchy;
    public Order Element => this;

    private bool _isInitialized;

    public void Init(OrderConfig orderConfig)
    {
        if (!_isInitialized) InitDependency();

        _orderConfig = orderConfig;
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

        TimeChangedDelegate = changedValue => ChangeRemainTime(changedValue);

        _timeController.OnTimeChanged += TimeChangedDelegate;

        _goalPool = ServiceLocator.Instance.Get<GoalPool>();
    }

    public void ApplyOrder()
    {
        if (!_isApplied)
        {
            Debug.Log("Order applied");
            _isApplied = true;

            _goal = _goalPool.Get();
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
        }
    }

    public void CompleteOrder()
    {
        if (_isApplied)
        {
            Debug.Log("Order completed");
            _isApplied = false;
            _rewardHandler.ApplyRewardForOrder(this);
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
        _timeController.OnTimeChanged -= TimeChangedDelegate;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Release()
    {
        gameObject.SetActive(false);
    }
}
