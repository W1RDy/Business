using UnityEngine;

public class ButtonForTutorialActivator : IService
{
    private ITutorialButton _currentTutorialButton;
    private ButtonsForTutorialService _service;

    public ButtonForTutorialActivator(ButtonsForTutorialService service)
    {
        _service = service;
    }

    public ITutorialButton ActivateButton(TutorialButtonType buttonType)
    {
        _currentTutorialButton = _service.GetTutorialButton(buttonType);
        if (buttonType == TutorialButtonType.ConstructPC) Debug.Log(_currentTutorialButton);
        _currentTutorialButton.ActivateByTutorial();
        return _currentTutorialButton;
    }

    public void DeactivateButton()
    {
        _currentTutorialButton.DeactivateByTutorial();
        _currentTutorialButton = null;
    }
}
