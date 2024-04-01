using UnityEngine;

[CreateAssetMenu(fileName = "DeliveryConfig", menuName = "DeliveryConfigs/New DeliveryConfig")]
public class DeliveryConfig : ScriptableObject
{
    [SerializeField] private DeliveryType _deliveryType;
    [SerializeField] private string _deliveryTitle;
    [SerializeField] private string _deliveryDescription;

    [SerializeField] private int _deliveryCost;
    [SerializeField] private int _deliveryTime;

    [SerializeField] private GoodsType _goodsType;

    public DeliveryType DeliveryType => _deliveryType;
    public string DeliveryTitle => _deliveryTitle;
    public string DeliveryDescription => _deliveryDescription;

    public int DeliveryCost => _deliveryCost;
    public int DeliveryTime => _deliveryTime;

    public GoodsType GoodsType => _goodsType;
}

public enum DeliveryType
{
    Low,
    Medium,
    High
}