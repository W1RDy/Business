using UnityEngine;

[CreateAssetMenu(fileName ="PCConfig", menuName = "PCConfig/New PCConfig")]
public class PCConfig : ScriptableObject
{
    [SerializeField] private GoodsType _goodsType;

    [SerializeField] private string _title;
    [SerializeField] private string _description;

    public GoodsType GoodsType => _goodsType;

    public string Title => _title;
    public string Description => _description;
}