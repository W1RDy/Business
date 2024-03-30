using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrderConfig", menuName = "OrderConfigs/New OrderConfig")]
public class OrderConfig : ScriptableObject
{
    [SerializeField] private int _id;
    [SerializeField] private int _cost;
    [SerializeField] private int _time;

    public int ID => _id;
    public int Cost => _cost;
    public int Time => _time;
}
