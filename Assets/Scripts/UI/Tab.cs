using UnityEngine;
using UnityEngine.UI;

public class Tab : OpenButton
{
    private ColorBlock _startColors;
    private bool _isTutorial;

    public override void Init()
    {
        base.Init();
        _startColors = _button.colors;
        (_window as WindowWithCallbacks).WindowChanged += ChangeInteractableDelegate;
        if (_windowType == WindowType.DeliveryWindow) OpenWindow();
    }

    protected override void OpenWindow()
    {
        if (_windowType == WindowType.InventoryWindow) _buttonService.OpenInventoryWindow();
        else _buttonService.OpenDeliveryWindow();
    }

    private void ChangeInteractableDelegate()
    {
        ChangeTabInteractable(!_window.gameObject.activeSelf);
    }

    protected override void ActivateClickableByTutorial()
    {
        _button.colors = _startColors;

        _isTutorial = false;
        ChangeInteractableDelegate();
    }

    protected override void DeactivateClickableByTutorial()
    {
        _isTutorial = true;

        ChangeTabInteractable(_button.interactable);
        _button.interactable = false;
    }

    public void ChangeTabInteractable(bool isInteractable)
    {
        if (!_isTutorial) _button.interactable = isInteractable;
        else
        {
            var colors = _button.colors;
            colors.disabledColor = isInteractable ? _startColors.normalColor : _startColors.disabledColor;

            _button.colors = colors;
        }
    }

    public void OnDestroy()
    {
        (_window as WindowWithCallbacks).WindowChanged -= ChangeInteractableDelegate;
    }
}