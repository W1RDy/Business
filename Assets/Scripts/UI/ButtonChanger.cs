using UnityEngine;

public class ButtonChanger
{
    [SerializeField] CustomButton _button1;
    [SerializeField] CustomButton _button2;

    public ButtonChanger(CustomButton button1, CustomButton button2)
    {
        _button1 = button1;
        _button2 = button2;
    }

    public void ChangeButtons(bool activateDefaultButton)
    {
        _button1.gameObject.SetActive(activateDefaultButton);
        _button2.gameObject.SetActive(!activateDefaultButton);
    }
}