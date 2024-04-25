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

    private ButtonTextFitter _buttonTextFitter;

    public override void Init()
    {
        base.Init();
        _buttonChangeController = ServiceLocator.Instance.Get<ButtonChangeController>();
        _conditionChecker = ServiceLocator.Instance.Get<GamesConditionChecker>();

        _buttonTextFitter = new ButtonTextFitter(_button.GetComponent<RectTransform>(), 15, 24);

        OnOrderValuesChanged = () => _buttonChangeController.ChangeButtonToNewButton(this);
        OnOrderStateChanged = () => _buttonChangeController.ChangeButtonStates(this);
            
        _order.OrderValuesChanged += OnOrderValuesChanged;
        _order.OrderStateChanged += OnOrderStateChanged;

        _buttonChangeController.AddChangeButton(this);
    }

    private void OnEnable()
    {
        if (OnOrderValuesChanged != null)
        {
            OnOrderStateChanged.Invoke();
            OnOrderValuesChanged.Invoke();
        }
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
        _button.interactable = !isApplied;
        var key = isApplied ? "Applied" : "Apply";
        SetText(key);
    }

    protected override void SetText(string key)
    {
        base.SetText(key);
        _buttonTextFitter.CheckButtonTextFit(_stateText);
    }

    public bool CheckButtonChangeCondition()
    {
        if (_order is Order standartOrder) return _order.IsApplied && _conditionChecker.IsHasInInventory(standartOrder.NeededGoods);
        if (_order is CompositeOrder compositeOrder) return !_conditionChecker.IsEnoughCoinsInHands(compositeOrder.Cost);
        return false;
    }

    protected override void ActivateClickableByTutorial()
    {
        base.ActivateClickableByTutorial();
        OnOrderStateChanged?.Invoke();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
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
