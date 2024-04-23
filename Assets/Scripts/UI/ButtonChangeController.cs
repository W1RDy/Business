using CoinsCounter;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChangeController : ClassForInitialization, IService, ISubscribable
{
    private Dictionary<ChangeCondition, List<IChangeButton>> _buttons = new Dictionary<ChangeCondition, List<IChangeButton>>();

    private GameController _gameController;
    private HandsCoinsCounter _handsCoinsCounter;
    private PCService _pcService;

    private SubscribeController _subscribeController;

    public ButtonChangeController() : base() { }

    public override void Init()
    {
        _gameController = ServiceLocator.Instance.Get<GameController>();
        _handsCoinsCounter = ServiceLocator.Instance.Get<HandsCoinsCounter>();
        _pcService = ServiceLocator.Instance.Get<PCService>();
        _subscribeController = ServiceLocator.Instance.Get<SubscribeController>();

        Subscribe();
    }

    public void AddChangeButton(IChangeButton button)
    {
        List<IChangeButton> buttonsList;

        foreach (var changeCondition in button.ChangeConditions)
        {
            if (!_buttons.TryGetValue(changeCondition, out buttonsList))
            {
                _buttons[changeCondition] = buttonsList = new List<IChangeButton>();
            }
            buttonsList.Add(button);
        }

        if (button is IButtonWithNewButton withNewButton) ChangeButtonToNewButton(withNewButton);
        if (button is IButtonWithStates withStates) ChangeButtonStates(withStates);
    }

    public void RemoveChangeButton(IChangeButton button)
    {
        foreach (var changeCondition in button.ChangeConditions)
        {
            if (_buttons.ContainsKey(changeCondition))
            {
                _buttons[changeCondition].Remove(button);
                if (_buttons[changeCondition].Count == 0) _buttons.Remove(changeCondition);
            }
        }
    }

    public void ChangeByCoinsChangeCondition()
    {
        ChangeButtons(ChangeCondition.CoinsChanged);
    }

    public void ChangeByNewItemCondition()
    {
        ChangeButtons(ChangeCondition.InventoryChanged);
    }

    public void ChangeByFinishGameCondition()
    {
        ChangeButtons(ChangeCondition.GameFinished);
    }

    public void ChangeButtons(ChangeCondition changeCondition)
    {
        if (_buttons.TryGetValue(changeCondition, out var buttons))
        {
            var buttonsCopy = new List<IChangeButton>(buttons);
            foreach (var button in buttonsCopy)
            {
                if (button is IButtonWithNewButton buttonWithNewButton) ChangeButtonToNewButton(buttonWithNewButton);
                if (button is IButtonWithStates buttonWithStates) ChangeButtonStates(buttonWithStates);
            }
        }
    }

    public void ChangeButtonToNewButton(IButtonWithNewButton button)
    {
        if (button.CheckButtonChangeCondition()) ChangeButton(button as CustomButton, button.ButtonForChange);
        else ChangeButton(button.ButtonForChange, button as CustomButton);
    }

    public void ChangeButtonStates(IButtonWithStates button)
    {
        if (button.CheckStatesChangeCondition()) button.ChangeStates(true);
        else button.ChangeStates(false);
    }

    private void ChangeButton(CustomButton from, CustomButton to)
    {
        if (from.gameObject.activeSelf)
        {
            from.gameObject.SetActive(false);
            to.gameObject.SetActive(true);
        }
    }

    public void Subscribe()
    {
        _subscribeController.AddSubscribable(this);

        _handsCoinsCounter.CoinsChanged += ChangeByCoinsChangeCondition;
        _gameController.GameFinished += ChangeByFinishGameCondition;
        _gameController.GameStarted += ChangeByFinishGameCondition;
        _pcService.ItemsUpdated += ChangeByNewItemCondition;
    }

    public void Unsubscribe()
    {
        _handsCoinsCounter.CoinsChanged -= ChangeByCoinsChangeCondition;
        _gameController.GameFinished -= ChangeByFinishGameCondition;
        _gameController.GameStarted -= ChangeByFinishGameCondition;
        _pcService.ItemsUpdated -= ChangeByNewItemCondition;
    }
}