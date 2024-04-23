using TMPro;
using UnityEngine;

public class ButtonTextFitter
{
    private bool _isChanged;

    private RectTransform _buttonTransform;

    public ButtonTextFitter(RectTransform buttonTransform)
    {
        _buttonTransform = buttonTransform;
    }

    public void CheckButtonTextFit(TextMeshProUGUI buttonText)
    {
        if (_buttonTransform.sizeDelta.x - buttonText.preferredWidth < 1f) ChangeTextFit(buttonText);
    }

    private void ChangeTextFit(TextMeshProUGUI buttonText)
    {
        if (!_isChanged)
        {
            buttonText.fontSize = buttonText.fontSize - 4f;
            _isChanged = true;
        }
    }
}