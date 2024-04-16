using UnityEngine;

public class Tab : OpenButton
{
    [SerializeField] private Tab _connectedTab; 

    public override void Init()
    {
        base.Init();
        if (_windowType == WindowType.InventoryWindow) OpenWindow();
    }

    protected override void OpenWindow()
    {
        if (_windowType == WindowType.InventoryWindow) _buttonService.OpenInventoryWindow();
        else _buttonService.OpenDeliveryWindow();

        ChangeTabInteractable(false);
        _connectedTab.ChangeTabInteractable(true);
    }

    public void ChangeTabInteractable(bool isInteractable)
    {
        _button.interactable = isInteractable;
    }
}