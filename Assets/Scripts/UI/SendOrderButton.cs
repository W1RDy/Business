using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendOrderButton : OrdersControlButton, IButtonWithStates
{
    [SerializeField] private ChangeCondition[] _changeConditions;
    public ChangeCondition[] ChangeConditions => _changeConditions;

    private ButtonChangeController _changeController;
    private GamesConditionChecker _conditionChecker;

    private ButtonTextFitter _buttonTextFitter;

    private Action OnOrderChanged;

    [SerializeField] private UIBlockWithStates _connectedBlockWithStates;

    public override void Init()
    {
        base.Init();

        _buttonTextFitter = new ButtonTextFitter(_button.GetComponent<RectTransform>());
        SetText("Send");

        _changeController = ServiceLocator.Instance.Get<ButtonChangeController>();
        _conditionChecker = ServiceLocator.Instance.Get<GamesConditionChecker>();

        OnOrderChanged = () => _changeController.ChangeButtonStates(this);

        _changeController.AddChangeButton(this);
    }
    
    protected override void ClickCallback()
    {
        base.ClickCallback();
        SendOrder();
    }

    public void SetOrder(IOrderWithCallbacks order)
    {
        _order = order;
        _order.OrderStateChanged += OnOrderChanged;
    }

    private void SendOrder()
    {
        _buttonService.SendOrder(_order);
    }

    protected override void ActivateClickableByTutorial()
    {
        base.ActivateClickableByTutorial();
        OnOrderChanged?.Invoke();
    }

    public void ChangeStates(bool toActiveState)
    {
        _button.interactable = toActiveState;
        if (_connectedBlockWithStates != null)
        {
            _connectedBlockWithStates.ChangeState(toActiveState);
        }
    }

    public bool CheckStatesChangeCondition()
    {
        if (_order == null) return false;
        return _conditionChecker.IsHasInInventory((_order as Order).NeededGoods);
    }

    protected override void SetText(string key)
    {
        base.SetText(key);
        _buttonTextFitter.CheckButtonTextFit(_stateText);
    }

    public void OnDestroy()
    {
        if (_changeController != null) _changeController.RemoveChangeButton(this);
        if (_order != null) _order.OrderStateChanged -= OnOrderChanged;
    }
}
