using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PCView
{
    private TextMeshProUGUI _titleText;
    private TextMeshProUGUI _descriptionText;

    private TextMeshProUGUI _amountText;

    private TextMeshProUGUI _brokenText;
    private GameObject _brokenViewContainer;

    private Image _iconSpace;

    public PCView(TextMeshProUGUI titleText, TextMeshProUGUI descriptionText, TextMeshProUGUI amountText, TextMeshProUGUI brokenText, Image iconSpace)
    {
        _titleText = titleText;
        _descriptionText = descriptionText;

        _amountText = amountText;

        _brokenText = brokenText;
        _brokenViewContainer = brokenText.transform.parent.gameObject;

        _iconSpace = iconSpace;
    }

    public void SetView(string title, string description, int amount, bool isBroken, Sprite icon)
    {
        _titleText.text = title;
        _descriptionText.text = description;

        SetAmount(amount);
        SetBroken(isBroken);
        SetIcon(icon);
    }

    public void SetAmount(int amount)
    {
        _amountText.text = "X" + amount.ToString();
    }

    public void SetBroken(bool isBroken)
    {
        if (isBroken)
        {
            if (_brokenText.text != "Broken") _brokenText.text = "Broken";
            _brokenViewContainer.SetActive(true);
        }
        else _brokenViewContainer.SetActive(false);
    }

    public void SetIcon(Sprite icon)
    {
        _iconSpace.sprite = icon;
    }
}
