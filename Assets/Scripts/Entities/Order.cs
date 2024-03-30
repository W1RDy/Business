using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OrderView))]
public class Order : MonoBehaviour, IOrder
{
    #region Values

    [SerializeField] private int _id;
    [SerializeField] private int _cost;
    [SerializeField] private int _time;

    private int _remainTime;

    public int ID => _id;
    public int Cost => _cost;
    public int Time => _time;

    #endregion

    private bool _isApplied;
    public bool IsApplied => _isApplied;

    private OrderView _view;

    private RewardHandler _rewardHandler;
    private TimeController _timeController;
    private ActiveOrderService _activeOrderService;

    private Action<int> TimeChangedDelegate;

    private Goal _goal;
    [SerializeField] RectTransform _goalFactory;
    private GoalPool _goalPool;

    private void Start()
    {
        _view = GetComponent<OrderView>();
        _view.SetView(_cost, _time);

        _remainTime = _time;

        _rewardHandler = ServiceLocator.Instance.Get<RewardHandler>();
        _timeController = ServiceLocator.Instance.Get<TimeController>();
        _activeOrderService = ServiceLocator.Instance.Get<ActiveOrderService>();

        TimeChangedDelegate = changedValue => ChangeRemainTime(changedValue);

        _timeController.OnTimeChanged += TimeChangedDelegate;

        var goalFactory = new GoalFactory(_goalFactory);
        goalFactory.LoadResources();

        _goalPool = new GoalPool(goalFactory, 2);
        _goalPool.Init();
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
        _remainTime = Mathf.Clamp(_remainTime - changeValue, 0, _time);

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
}
