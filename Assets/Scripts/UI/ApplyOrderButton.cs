using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApplyOrderButton : OrdersControlButton, IChangeButton
{
    [SerializeField] private CustomButton _buttonForChange;
    public CustomButton ButtonForChange => _buttonForChange;

    private ButtonChangeController _buttonChangeController;

    private GamesConditionChecker _conditionChecker;
    private Action OnOrderChanged;

    protected override void Start()
    {
        base.Start();
        ChangeState(false);
    }

    protected override void Init()
    {
        base.Init();
        _buttonChangeController = ServiceLocator.Instance.Get<ButtonChangeController>();
        _conditionChecker = ServiceLocator.Instance.Get<GamesConditionChecker>();

        OnOrderChanged = () => _conditionChecker.CheckHandsCoinsConditions();
        _order.OrderChanged += OnOrderChanged;

        _buttonChangeController.AddChangeButton(this);
    }

    protected override void ClickCallback()
    {
        base.ClickCallback();
        ApplyOrder();
    }

    private void ApplyOrder()
    {
        _buttonService.ApplyOrder(_order);
    }

    public void ChangeState(bool isApplied)
    {
        if (_button == null) Start();
        _button.interactable = !isApplied;
        var text = isApplied ? "Applied" : "Apply";
        SetText(text);
    }

    public bool CheckChangeCondition()
    {
        if (_order is Order standartOrder) return _conditionChecker.IsHasInInventory(standartOrder.NeededGoods);
        if (_order is CompositeOrder compositeOrder) return !_conditionChecker.IsEnoughCoins(compositeOrder.Cost);
        return false;
    }

    public void OnDestroy()
    {
        _buttonChangeController.RemoveChangeButton(this);
        _order.OrderChanged -= OnOrderChanged;
    }
}