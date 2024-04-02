using TMPro;
using UnityEngine;

public class Goods : MonoBehaviour, IPoolElement<Goods>
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

    #endregion

    public bool IsBroken { get; private set; }

    public bool IsFree { get; private set; }
    public Goods Element => this;

    private PCGenerator _pcGenerator;
    private Pool<Goods> _pool;

    public void Init(GoodsConfig config, bool isBroken, int amount)
    {
        _config = config;
        IsBroken = isBroken;

        if (_view == null) _view = new GoodsView(_titleText, _descriptionText, _timeText, _amountText);
        Amount = amount;
        _view.SetView(_config.Title, _config.Description, _config.BuildTime, Amount);
    }

    private void Start()
    {
        _pcGenerator = ServiceLocator.Instance.Get<PCGenerator>();
        _pool = ServiceLocator.Instance.Get<Pool<Goods>>();
    }

    public void ConstructPC()
    {
        _pcGenerator.GeneratePC(_config.GoodsType, IsBroken);
        Amount -= 1;
        if (Amount == 0) _pool.Release(this);
    }

    public void Activate()
    {
        IsFree = false;
        gameObject.SetActive(true);
    }

    public void Release()
    {
        IsFree = true;
        gameObject.SetActive(false);
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
        _timeText.text = "Time: " + time.ToString();
    }

    public void SetAmount(int amount)
    {
        _amountText.text = "X" + amount;
    }
}