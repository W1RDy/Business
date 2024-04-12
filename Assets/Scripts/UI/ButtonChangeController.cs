using CoinsCounter;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChangeController : IService
{
    private Dictionary<ChangeCondition, List<IChangeButton>> _buttons = new Dictionary<ChangeCondition, List<IChangeButton>>();

    private Action InitDelegate;

    public ButtonChangeController()
    {
        InitDelegate = () =>
        {
            var gameController = ServiceLocator.Instance.Get<GameController>();
            var handsCoinsCounter = ServiceLocator.Instance.Get<HandsCoinsCounter>();
            var pcService = ServiceLocator.Instance.Get<PCService>();

            handsCoinsCounter.CoinsChanged += ChangeByCoinsChangeCondition;
            gameController.GameFinished += ChangeByFinishGameCondition;
            pcService.ItemsUpdated += ChangeByNewItemCondition;

            ServiceLocator.Instance.ServiceRegistered -= InitDelegate;
        };
        ServiceLocator.Instance.ServiceRegistered += InitDelegate;
        if (ServiceLocator.Instance.IsRegistered) InitDelegate.Invoke();
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
            foreach (var button in buttons)
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
}