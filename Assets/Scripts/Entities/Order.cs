using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OrderView))]
public class Order : MonoBehaviour, IOrder
{
    #region Values

    [SerializeField] private int _id;
    [SerializeField] private int _cost;
    [SerializeField] private int _time;

    public int ID => _id;
    public int Cost => _cost;
    public int Time => _time;

    #endregion

    private bool _isApplied;
    public bool IsApplied => _isApplied;

    private OrderView _view;

    private RewardHandler _rewardHandler;

    private void Start()
    {
        _view = GetComponent<OrderView>();
        _view.SetView(_cost, _time);

        _rewardHandler = ServiceLocator.Instance.Get<RewardHandler>();
    }

    public void ApplyOrder()
    {
        if (!_isApplied)
        {
            Debug.Log("Order applied");
            _isApplied = true;
        }
    }

    public void CancelOrder()
    {
        if (_isApplied)
        {
            Debug.Log("Order canceled");
            _isApplied = false;
        }
    }

    public void CompleteOrder()
    {
        if (_isApplied)
        {
            Debug.Log("Order completed");
            _isApplied = false;
            _rewardHandler.ApplyRewardForOrder(this);
        }
    }
}
