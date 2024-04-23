using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OrderView))]
public class Order : ObjectForInitialization, IRemembable, IOrderWithCallbacks, IThrowable, IPoolElement<Order>
{
    #region Values

    private OrderInstanceConfig _orderConfig;

    private int _id;
    private int _remainTime;
    private int _remainWaiting;

    public int ID => _id;
    public int Cost => _orderConfig.Cost;
    public int Time => _orderConfig.Time;
    public GoodsType NeededGoods => _orderConfig.NeededGoods;
    public int RemainWaiting => _remainWaiting;

    public OrderInstanceConfig OrderConfig => _orderConfig;

    #endregion

    #region View

    [SerializeField] private UIAnimation _appearAnimation;
    [SerializeField] private UIAnimation _disappearAnimation;
    private EntityAnimationsController _animController;

    #endregion

    private bool _isApplied;
    public bool IsApplied => _isApplied;

    private OrderView _view;

    protected RewardHandler _rewardHandler;
    private TimeController _timeController;

    protected ActiveOrderService _activeOrderService;
    protected OrderService _orderService;
    protected OrderCompleteHandler _orderCompleteHandler;
    private RememberedOrderService _rememberedOrderService;
    private NotificationController _notificationController;

    private Action<int> TimeChangedDelegate;

    protected Goal _goal;
    protected Pool<Goal> _goalPool;
    protected Pool<Order> _orderPool;

    private bool _isFree;
    public bool IsFree => _isFree;
    public Order Element => this;

    public event Action OrderValuesChanged;
    public event Action OrderStateChanged;

    protected AudioPlayer _audioPlayer;

    protected IIDGenerator _idGenerator;

    public override void Init()
    {
        base.Init();

        gameObject.SetActive(false);
        _isFree = true;

        _view = GetComponent<OrderView>();

        _rewardHandler = ServiceLocator.Instance.Get<RewardHandler>();
        _timeController = ServiceLocator.Instance.Get<TimeController>();
        _activeOrderService = ServiceLocator.Instance.Get<ActiveOrderService>();
        _orderService = ServiceLocator.Instance.Get<OrderService>();
        _orderCompleteHandler = ServiceLocator.Instance.Get<OrderCompleteHandler>();
        _rememberedOrderService = ServiceLocator.Instance.Get<RememberedOrderService>();
        _notificationController = ServiceLocator.Instance.Get<NotificationController>();

        _goalPool = ServiceLocator.Instance.Get<Pool<Goal>>();
        _orderPool = ServiceLocator.Instance.Get<Pool<Order>>();

        TimeChangedDelegate = changedValue => ChangeRemainTime(changedValue);

        _timeController.OnTimeChanged += TimeChangedDelegate;

        _audioPlayer = ServiceLocator.Instance.Get<AudioPlayer>();

        InitAnimations();
    }
    
    public void InitVariant(int id, OrderInstanceConfig orderConfig, IIDGenerator idGenerator, bool isApplied, int remaimWaiting)
    {
        _orderConfig = orderConfig;
        _id = id;
        _remainWaiting = remaimWaiting;

        _view.SetView(_orderConfig.Cost, _orderConfig.Time, NeededGoods, ID);

        _remainTime = _orderConfig.Time;
        _notificationController.AddNotification(this);

        _idGenerator = idGenerator;

        if (isApplied) ApplyOrder();
    }

    public void InitVariant(int id, OrderInstanceConfig orderConfig, IIDGenerator idGenerator)
    {
        InitVariant(id, orderConfig, idGenerator, false, 2);
    }

    private void InitAnimations()
    {
        _animController = new EntityAnimationsController(_appearAnimation, _disappearAnimation, gameObject);
    }

    public void ApplyOrder()
    {
        if (!_isApplied)
        {
            _isApplied = true;

            _goal = _goalPool.Get();
            _goal.InitVariant(this);

            _notificationController.RemoveNotification(this);

            OrderStateChanged?.Invoke();
            OrderValuesChanged?.Invoke();
        }
    }

    public void UpdateOrderUrgency()
    {
        if (!_isApplied)
        {
            _remainWaiting--;
            if (_remainWaiting == 2)
            {
                _view.ChangeBorders(true);
            }
            else if (_remainWaiting == 1)
            {
                _view.ChangeBorders(false);
            }
            else if (_remainWaiting <= 0) ThrowOut();
        }
        else _view.ChangeBorders(true);
    }

    public void CancelOrder()
    {
        if (_isApplied)
        {
            _orderPool.Release(this);
            _audioPlayer.PlaySound("Cancel");
        }
    }

    public virtual void CompleteOrder()
    {
        if (_isApplied)
        {
            Debug.Log("CompleteOrder");
            _rewardHandler.ApplyRewardForOrder(this);
            _rememberedOrderService.RememberOrder(this);

            Action callback = () => _orderPool.Release(this);
            _animController.PlayDisappearAnimation(callback);

            _orderCompleteHandler.CompleteOrder(this);

            _audioPlayer.PlaySound("CompleteTask");
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

        if (_animController != null) _animController.PlayAppearAnimation();
    }

    public virtual void Release()
    {
        gameObject.SetActive(false);
        _orderService.RemoveOrder(this);
        _idGenerator.ReleaseID(ID);

        if (_isApplied)
        {
            _activeOrderService.RemoveOrder(this);
            _goalPool.Release(_goal);
        }

        _isFree = true;
        _isApplied = false;
        OrderStateChanged?.Invoke();
        OrderValuesChanged?.Invoke();
    }

    public void ThrowOut()
    {
        if (_isApplied) CancelOrder();
        else
        {
            _notificationController.RemoveNotification(this);

            Action callback = () => _orderPool.Release(this);
            _animController.PlayDisappearAnimation(callback);
        }
    }

    private void OnDisable()
    {
        if (_animController != null) _animController.KillAnimation();
    }
}