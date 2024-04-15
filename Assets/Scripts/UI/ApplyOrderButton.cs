using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApplyOrderButton : OrdersControlButton, IButtonWithNewButton, IButtonWithStates
{
    [SerializeField] private CustomButton _buttonForChange;
    [SerializeField] private ChangeCondition[] _changeConditions;

    public ChangeCondition[] ChangeConditions => _changeConditions;
    public CustomButton ButtonForChange => _buttonForChange;

    private ButtonChangeController _buttonChangeController;

    private GamesConditionChecker _conditionChecker;
    private Action OnOrderValuesChanged;
    private Action OnOrderStateChanged;

    private bool _isInitialized;
 
    protected override void Start()
    {
        if (!_isInitialized)
        {
            _isInitialized = true;
            base.Start();
        }
    }

    protected override void Init()
    {
        base.Init();
        _buttonChangeController = ServiceLocator.Instance.Get<ButtonChangeController>();
        _conditionChecker = ServiceLocator.Instance.Get<GamesConditionChecker>();

        OnOrderValuesChanged = () => _buttonChangeController.ChangeButtonToNewButton(this);
        OnOrderStateChanged = () => _buttonChangeController.ChangeButtonStates(this);
            
        _order.OrderValuesChanged += OnOrderValuesChanged;
        _order.OrderStateChanged += OnOrderStateChanged;

        _buttonChangeController.AddChangeButton(this);
    }

    public void InitializeButton()
    {
        Start();
    }

    protected override void ClickCallback()
    {
        base.ClickCallback();
        ApplyOrder();
    }

    private void ApplyOrder()
    {
        if (_order as Order != null) _buttonService.ApplyOrder(_order);
        if (_order as CompositeOrder != null) _buttonService.ApplyOrderWithConfirm(_order);
    }

    public void ChangeStates(bool isApplied)
    {
        if (_button == null) Start();
        _button.interactable = !isApplied;
        var text = isApplied ? "Applied" : "Apply";
        SetText(text);
    }

    public bool CheckButtonChangeCondition()
    {
        if (_order is Order standartOrder) return _order.IsApplied && _conditionChecker.IsHasInInventory(standartOrder.NeededGoods);
        if (_order is CompositeOrder compositeOrder) return !_conditionChecker.IsEnoughCoinsInHands(compositeOrder.Cost);
        return false;
    }

    public void OnDestroy()
    {
        if (_buttonChangeController != null)
        {
            _buttonChangeController.RemoveChangeButton(this);
            _order.OrderValuesChanged -= OnOrderValuesChanged;
            _order.OrderStateChanged -= OnOrderStateChanged;
        }
    }

    public bool CheckStatesChangeCondition()
    {
        return _order.IsApplied;
    }
}