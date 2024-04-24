using TMPro;
using UnityEngine;

public class ButtonTextFitter
{
    private float offset = 0.5f;

    private float _minSize;
    private float _maxSize;

    private RectTransform _buttonTransform;

    public ButtonTextFitter(RectTransform buttonTransform, float minSize, float maxSize)
    {
        _buttonTransform = buttonTransform;

        _minSize = minSize;
        _maxSize = maxSize;
    }

    public void CheckButtonTextFit(TextMeshProUGUI buttonText)
    {
        if (buttonText.preferredWidth > _buttonTransform.sizeDelta.x - offset) ChangeTextFont(buttonText);
        else if (buttonText.preferredWidth < buttonText.maxWidth - (offset + 1) && buttonText.fontSize < _maxSize) ChangeTextFont(buttonText);
    }

    private void ChangeTextFont(TextMeshProUGUI buttonText)
    {
        var fontSize = Mathf.Clamp(CalculateNewFontSize(buttonText, buttonText.text, buttonText.fontSize), _minSize, _maxSize);
        buttonText.fontSize = fontSize;
    }

    private float CalculateNewFontSize(TextMeshProUGUI text, string message, float fontSize)
    {
        var neededAverageWidth = (_buttonTransform.sizeDelta.x / message.Length) - offset;
        var currentAverageWidth = text.preferredWidth / message.Length;

        var divideValue = neededAverageWidth / currentAverageWidth;
        return fontSize * divideValue;
    }
}