using UnityEngine;

[CreateAssetMenu(fileName = "GoodsConfigs", menuName = "GoodsConfigs/New GoodsConfigs")]
public class GoodsConfig : ScriptableObject
{ 
    [SerializeField] private GoodsType _goodsType;

    [SerializeField] private string _title;
    [SerializeField] private string _description;

    [SerializeField] private int _buildTime;

    public GoodsType GoodsType => _goodsType;
    public string Title => _title;
    public string Description => _description;

    public int BuildTime => _buildTime;
}

public enum GoodsType
{
    LowQuality,
    MediumQuality,
    HighQuality
}