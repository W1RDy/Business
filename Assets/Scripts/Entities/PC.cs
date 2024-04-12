using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PC : MonoBehaviour, IThrowable, IPoolElement<PC>
{
    #region Values

    public int ID { get; private set; }

    private PCConfig _config;

    private int _amount;
    public int Amount
    {
        get => _amount;
        set
        {
            _amount = value;
            _view.SetAmount(value);
        }
    }

    public GoodsType QualityType => _config.GoodsType;

    public bool IsBroken => _config.IsBroken;

    #endregion

    #region View

    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _descriptionText;

    [SerializeField] private TextMeshProUGUI _amountText;

    [SerializeField] private TextMeshProUGUI _brokenText;

    [SerializeField] private UIAnimation _appearAnimation;
    [SerializeField] private UIAnimation _disappearAnimation;

    [SerializeField] private Image _iconSpace;
 
    private EntityAnimationsController _animController;

    private PCView _view;

    #endregion

    private bool _isBroken;
    public bool IsFree { get; private set; } 
    public PC Element => this;

    private PCService _service;
    private Pool<PC> _pool;

    private Action InitDelegate;

    public void InitInstance()
    {
        InitDelegate = () =>
        {
            Release();

            _service = ServiceLocator.Instance.Get<PCService>();
            _pool = ServiceLocator.Instance.Get<Pool<PC>>();


            InitAnimations();

            _view = new PCView(_titleText, _descriptionText, _amountText, _brokenText, _iconSpace);

            ServiceLocator.Instance.ServiceRegistered -= InitDelegate;
        };

        ServiceLocator.Instance.ServiceRegistered += InitDelegate;
        if (ServiceLocator.Instance.IsRegistered) InitDelegate.Invoke();
    }

    public void InitVariant(PCConfig config, bool isBroken)
    {
        ID = config.ID;
        _config = config;
        _isBroken = isBroken;

        Amount = 1;
        _view.SetView(_config.Title, _config.Description, Amount, _isBroken, _config.Icon);
    }

    private void InitAnimations() => _animController = new EntityAnimationsController(_appearAnimation, _disappearAnimation, gameObject);

    public void Activate()
    {
        IsFree = false;
        gameObject.SetActive(true);

        _animController.PlayAppearAnimation();
    }

    public void Release()
    {
        IsFree = true;
        gameObject.SetActive(false);
        if (_service != null) _service.RemovePC(this);
    }

    public void ThrowOut()
    {
        Amount -= 1;
        if (Amount == 0)
        {
            Action callback = () => _pool.Release(this);
            _animController.PlayDisappearAnimation(callback);
        }
    }

    private void OnDisable()
    {
        if (_animController != null) _animController.KillAnimation();
    }
}
