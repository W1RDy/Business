using TMPro;

public class PCView
{
    private TextMeshProUGUI _titleText;
    private TextMeshProUGUI _descriptionText;

    private TextMeshProUGUI _amountText;

    public PCView(TextMeshProUGUI titleText, TextMeshProUGUI descriptionText, TextMeshProUGUI amountText)
    {
        _titleText = titleText;
        _descriptionText = descriptionText;

        _amountText = amountText;
    }

    public void SetView(string title, string description, int amount)
    {
        _titleText.text = title;
        _descriptionText.text = description;

        SetAmount(amount);
    }

    public void SetAmount(int amount)
    {
        _amountText.text = "X" + amount.ToString();
    }
}
