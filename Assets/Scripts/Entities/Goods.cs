using System;
using TMPro;
using UnityEngine;

public class Goods : MonoBehaviour, IThrowable, IPoolElement<Goods>
{
    #region Values

    public int ID => (int)_config.GoodsType;

    public int Time => _config.BuildTime;

    private GoodsConfig _config;


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

    #endregion

    #region View

    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _descriptionText;

    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _amountText;

    private GoodsView _view;

    [SerializeField] private UIAnimation _appearAnimation;
    [SerializeField] private UIAnimation _disappearAnimation;
    private EntityAnimationsController _animController;

    #endregion

    public bool IsBroken { get; private set; }

    public bool IsFree { get; private set; }
    public Goods Element => this;

    private GoodsService _goodsService;
    private PCGenerator _pcGenerator;
    private Pool<Goods> _pool;

    public void InitInstance()
    {
        Release();

        _view = new GoodsView(_titleText, _descriptionText, _timeText, _amountText);
        InitAnimations();
    }

    public void InitVariant(GoodsConfig config, bool isBroken, int amount)
    {
        _config = config;
        IsBroken = isBroken;

        Amount = amount;
        _view.SetView(_config.Title, _config.Description, _config.BuildTime, Amount);
    }

    private void InitAnimations() => _animController = new EntityAnimationsController(_appearAnimation, _disappearAnimation, gameObject);

    private void Start()
    {
        _pcGenerator = ServiceLocator.Instance.Get<PCGenerator>();
        _pool = ServiceLocator.Instance.Get<Pool<Goods>>();

        _goodsService = ServiceLocator.Instance.Get<GoodsService>();
    }

    public void ConstructPC()
    {
        _pcGenerator.GeneratePC(_config.GoodsType, IsBroken);
        Amount -= 1;
        if (Amount == 0)
        {
            Action callback = () => _pool.Release(this);
            _animController.PlayDisappearAnimation(callback);
        }
    }

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

        if (_goodsService != null) _goodsService.RemoveGoods(this);
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

public class GoodsView
{
    private TextMeshProUGUI _titleText;
    private TextMeshProUGUI _descriptionText;
    
    private TextMeshProUGUI _timeText;
    private TextMeshProUGUI _amountText;

    public GoodsView(TextMeshProUGUI titleText, TextMeshProUGUI descriptionText, TextMeshProUGUI timeText, TextMeshProUGUI amountText)
    {
        _titleText = titleText;
        _descriptionText = descriptionText;
        _timeText = timeText;
        _amountText = amountText;
    }

    public void SetView(string title, string description, int time, int amount)
    {
        _titleText.text = title;
        _descriptionText.text = description;

        SetTime(time);
        SetAmount(amount);
    }

    public void SetTime(int time)
    {
        _timeText.text = "+ " + time.ToString();
    }

    public void SetAmount(int amount)
    {
        _amountText.text = "X" + amount;
    }
}