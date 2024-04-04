using TMPro;
using UnityEngine;

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

    private PCView _view;

    #endregion

    private bool _isBroken;
    public bool IsFree { get; private set; } 
    public PC Element => this;

    private Pool<PC> _pool;

    public void Init(PCConfig config, bool isBroken)
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

    public void ThrowOut()
    {
        Amount -= 1;
        if (Amount == 0) _pool.Release(this);
    }
}
