using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SendOrderButton : OrdersControlButton, IButtonWithStates
{
    [SerializeField] private ChangeCondition[] _changeConditions;
    public ChangeCondition[] ChangeConditions => _changeConditions;

    private ButtonChangeController _changeController;
    private GamesConditionChecker _conditionChecker;

    private Action OnOrderChanged;
    private bool _isInitialized;

    protected override void Init()
    {
        if (_order != null && !_isInitialized) SetOrder(_order); 
    }
    
    private void InitializeInstance()
    {
        if (!_isInitialized)
        {
            _isInitialized = true;
            base.Init();

            SetText("Send");

            _changeController = ServiceLocator.Instance.Get<ButtonChangeController>();
            _conditionChecker = ServiceLocator.Instance.Get<GamesConditionChecker>();

            OnOrderChanged = () => _changeController.ChangeButtonStates(this);

            _changeController.AddChangeButton(this);
        }
    }

    protected override void ClickCallback()
    {
        base.ClickCallback();
        Debug.Log(transform.parent.parent.name);
        Debug.Log("Click");
        SendOrder();
    }

    public void SetOrder(IOrderWithCallbacks order)
    {
        _order = order;
        InitializeInstance();
        _order.OrderStateChanged += OnOrderChanged;
    }

    private void SendOrder()
    {
        _buttonService.SendOrder(_order);
    }

    public void ChangeStates(bool toActiveState)
    {
        _button.interactable = toActiveState;
    }

    public bool CheckStatesChangeCondition()
    {
        return _conditionChecker.IsHasInInventory((_order as Order).NeededGoods);
    }

    public void OnDestroy()
    {
        if (_changeController != null) _changeController.RemoveChangeButton(this);
        if (_order != null) _order.OrderStateChanged -= OnOrderChanged;
    }
}
