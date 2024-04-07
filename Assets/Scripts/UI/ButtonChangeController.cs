using System;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChangeController : IService
{
    private Dictionary<Type, List<IChangeButton>> _buttons = new Dictionary<Type, List<IChangeButton>>();

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

    public void ChangeButtonsToSendButtons()
    {
        ChangeButtons(typeof(SendOrderButton));
    }

    public void ChangeButtonsToDistributeButtons()
    {
        ChangeButtons(typeof(OpenDistributeCoinsButton));
    }

    public void ChangeButtons(Type buttonForChangeType)
    {
        foreach (var button in _buttons[buttonForChangeType])
        {
            if (button.CheckChangeCondition()) ChangeButton(button as CustomButton, button.ButtonForChange);
            else ChangeButton(button.ButtonForChange, button as CustomButton);
        }
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