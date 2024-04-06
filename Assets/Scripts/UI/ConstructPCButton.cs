using TMPro;
using UnityEngine;

public class ConstructPCButton : CustomButton
{
    [SerializeField] protected Goods _goods;
    [SerializeField] protected TextMeshProUGUI _text;

    private void Awake()
    {
        SetText("Construct");
    }

    protected override void ClickCallback()
    {
        _buttonService.ConstructPC(_goods);
    }

    protected void SetText(string text)
    {
        _text.text = text;
    }
}
