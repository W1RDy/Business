using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName ="PCConfig", menuName = "PCConfig/New PCConfig")]
public class PCConfig : ScriptableObject
{
    [SerializeField] private int _id;

    [SerializeField] private GoodsType _goodsType;

    [SerializeField] private string _title;
    [SerializeField] private string _description;

    [SerializeField] private bool _isReturned;
    [SerializeField] private bool _isBroken;

    [SerializeField] private Sprite _icon;

    public int ID => _id;

    public GoodsType GoodsType => _goodsType;

    public string Title => _title;
    public string Description => _description;
    public bool IsReturned => _isReturned;
    public bool IsBroken => _isBroken;

    public Sprite Icon => _icon;
}
