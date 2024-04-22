using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Goods : ObjectForInitialization, IRemembable, IThrowable, IPoolElement<Goods>
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

    public bool IsTutorialGoods { get; private set; }

    #endregion

    #region View

    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _descriptionText;

    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _amountText;

    [SerializeField] private Image _icon;

    private GoodsView _view;

    [SerializeField] private UIAnimation _appearAnimation;
    [SerializeField] private UIAnimation _disappearAnimation;
    private EntityAnimationsController _animController;

    #endregion

    public int BrokenGoodsCount { get; private set; }

    public bool IsFree { get; private set; }
    public Goods Element => this;

    private GoodsService _goodsService;
    private PCGenerator _pcGenerator;
    private Pool<Goods> _pool;

    public override void Init()
    {
        base.Init();
        Release();

        _view = new GoodsView(_titleText, _descriptionText, _timeText, _amountText, _icon);

        _pcGenerator = ServiceLocator.Instance.Get<PCGenerator>();
        _pool = ServiceLocator.Instance.Get<Pool<Goods>>();

        _goodsService = ServiceLocator.Instance.Get<GoodsService>();
        InitAnimations();
    }

    public void InitVariant(GoodsConfig config, int brokenGoodsCount, int amount)
    {
        _config = config;
        BrokenGoodsCount = brokenGoodsCount;

        Amount = amount;
        _view.SetView(_config.Title, _config.Description, _config.BuildTime, Amount, config.Icon);
    }

    private void InitAnimations() => _animController = new EntityAnimationsController(_appearAnimation, _disappearAnimation, gameObject);

    public void ConstructPC()
    {
        _pcGenerator.GeneratePC(_config.GoodsType, IsBreakGoods(), false);
        Amount -= 1;
        if (Amount == 0)
        {
            Action callback = () => _pool.Release(this);
            _animController.PlayDisappearAnimation(callback);
        }
    }

    private bool IsBreakGoods()
    {
        int randomGoodsIndex = Random.Range(1, Amount);

        var isBroken = randomGoodsIndex <= BrokenGoodsCount;
        if (isBroken) BrokenGoodsCount -= 1;
        return isBroken;
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

    private void OnDisable()
    {
        if (_animController != null) _animController.KillAnimation();
    }
}

public class GoodsView
{
    private TextMeshProUGUI _titleText;
    private TextMeshProUGUI _descriptionText;
    
    private TextMeshProUGUI _timeText;
    private TextMeshProUGUI _amountText;

    private Image _icon;

    public GoodsView(TextMeshProUGUI titleText, TextMeshProUGUI descriptionText, TextMeshProUGUI timeText, TextMeshProUGUI amountText, Image icon)
    {
        _titleText = titleText;
        _descriptionText = descriptionText;
        _timeText = timeText;
        _amountText = amountText;
        _icon = icon;
    }

    public void SetView(string title, string description, int time, int amount, Sprite icon)
    {
        _titleText.text = title;
        _descriptionText.text = description;

        SetTime(time);
        SetAmount(amount);
        SetIcon(icon);
    }

    public void SetTime(int time)
    {
        _timeText.text = "+ " + time.ToString();
    }

    public void SetAmount(int amount)
    {
        _amountText.text = "X" + amount;
    }

    public void SetIcon(Sprite icon)
    {
        _icon.sprite = icon;
    }
}