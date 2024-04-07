using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOrder
{
    public int ID { get; }
    public int Cost { get; }
    public int Time { get; }
    public bool IsApplied { get; }
    public event Action OrderChanged;

    public void ApplyOrder();
    public void CancelOrder();
    public void CompleteOrder();
}
