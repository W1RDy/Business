using CoinsCounter;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChangeController : IService
{
    private Dictionary<Type, List<IChangeButton>> _buttons = new Dictionary<Type, List<IChangeButton>>();

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
        if (!_buttons.TryGetValue(button.ButtonForChange.GetType(), out buttonsList))
        {
            _buttons[button.ButtonForChange.GetType()] = buttonsList = new List<IChangeButton>();
        }
        buttonsList.Add(button);
    }

    public void RemoveChangeButton(IChangeButton button)
    {
        if (_buttons.ContainsKey(button.ButtonForChange.GetType()))
        {
            _buttons[button.ButtonForChange.GetType()].Remove(button);
            if (_buttons[button.ButtonForChange.GetType()].Count == 0) _buttons.Remove(button.ButtonForChange.GetType());
        }
    }

    public void ChangeByCoinsChangeCondition()
    {
        ChangeButtons(typeof(OpenDistributeCoinsButton));
    }

    public void ChangeByNewItemCondition()
    {
        ChangeButtons(typeof(SendOrderButton));
    }

    public void ChangeByFinishGameCondition()
    {
        ChangeButtons(typeof(RestartButton));
    }

    public void ChangeButtons(Type buttonForChangeType)
    {
        if (_buttons.TryGetValue(buttonForChangeType, out var buttons))
        {
            foreach (var button in buttons)
            {
                ChangeButton(button);
            }
        }
    }

    public void ChangeButton(IChangeButton button)
    {
        if (button.CheckChangeCondition()) ChangeButton(button as CustomButton, button.ButtonForChange);
        else ChangeButton(button.ButtonForChange, button as CustomButton);
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