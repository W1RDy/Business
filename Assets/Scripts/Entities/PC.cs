using TMPro;
using UnityEngine;

public class PC : MonoBehaviour, IPoolElement<PC>
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

    public void Init(PCConfig config, bool isBroken)
    {
        _config = config;
        _isBroken = isBroken;

        if (_view == null) _view = new PCView(_titleText, _descriptionText, _amountText);
        Amount = 1;
        _view.SetView(_config.Title, _config.Description, Amount);
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
