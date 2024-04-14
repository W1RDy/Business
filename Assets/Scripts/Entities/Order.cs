using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OrderView))]
public class Order : MonoBehaviour, IRemembable, IOrderWithCallbacks, IThrowable, IPoolElement<Order>
{
    #region Values

    [SerializeField] private OrderConfig _orderConfig;

    private int _id;
    private int _remainTime;

    public int ID => _id;
    public int Cost => _orderConfig.Cost;
    public int Time => _orderConfig.Time;
    public GoodsType NeededGoods => _orderConfig.NeededGoods;

    public OrderConfig OrderConfig => _orderConfig;

    #endregion

    #region View

    [SerializeField] private UIAnimation _appearAnimation;
    [SerializeField] private UIAnimation _disappearAnimation;
    private EntityAnimationsController _animController;

    #endregion

    private bool _isApplied;
    public bool IsApplied => _isApplied;

    private OrderView _view;

    private RewardHandler _rewardHandler;
    private TimeController _timeController;

    private ActiveOrderService _activeOrderService;
    private OrderService _orderService;
    private OrderCompleteHandler _orderCompleteHandler;
    private RememberedOrderService _rememberedOrderService;
    private NotificationController _notificationController;

    private Action<int> TimeChangedDelegate;

    private Goal _goal;
    private Pool<Goal> _goalPool;
    private Pool<Order> _orderPool;

    private bool _isFree;
    public bool IsFree => _isFree;
    public Order Element => this;

    private Action InitDelegate;
    public event Action OrderValuesChanged;
    public event Action OrderStateChanged;

    private AudioPlayer _audioPlayer;

    private IIDGenerator _idGenerator;

    public void InitInstance()
    {
        InitDelegate = () =>
        {
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
            ServiceLocator.Instance.ServiceRegistered -= InitDelegate;
        };

        ServiceLocator.Instance.ServiceRegistered += InitDelegate;
        if (ServiceLocator.Instance.IsRegistered) InitDelegate.Invoke();
    }

    public void InitVariant(int id, OrderConfig orderConfig, IIDGenerator idGenerator)
    {
        _orderConfig = orderConfig;
        _id = id;

        _view.SetView(_orderConfig.Cost, _orderConfig.Time, NeededGoods, ID);

        _remainTime = _orderConfig.Time;
        _notificationController.AddNotification(this);

        _idGenerator = idGenerator;
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

    public void CancelOrder()
    {
        if (_isApplied)
        {
            _orderPool.Release(this);
            _audioPlayer.PlaySound("Cancel");
        }
    }

    public void CompleteOrder()
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

    public void Release()
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
