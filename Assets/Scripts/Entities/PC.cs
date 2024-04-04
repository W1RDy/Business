using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PC : MonoBehaviour, IThrowable, IPoolElement<PC>
{
    #region Values

    public int ID => (int)_config.GoodsType;

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

    #endregion

    #region View

    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _descriptionText;

    [SerializeField] private TextMeshProUGUI _amountText;

    [SerializeField] private UIAnimation _appearAnimation;
    [SerializeField] private UIAnimation _disappearAnimation;
    private EntityAnimationsController _animController;

    private PCView _view;

    #endregion

    private bool _isBroken;
    public bool IsFree { get; private set; } 
    public PC Element => this;

    private Pool<PC> _pool;

    public void InitInstance()
    {
        Release();
        InitAnimations();
    }

    public void InitVariant(PCConfig config, bool isBroken)
    {
        _config = config;
        _isBroken = isBroken;

        if (_view == null) _view = new PCView(_titleText, _descriptionText, _amountText);
        Amount = 1;
        _view.SetView(_config.Title, _config.Description, Amount);
    }

    private void Start()
    {
        _pool = ServiceLocator.Instance.Get<Pool<PC>>();

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
}
