using TMPro;
using UnityEngine;

public class ConstructPCButton : CustomButton
{
    [SerializeField] protected Goods _goods;
    [SerializeField] protected TextMeshProUGUI _text;

    public override void Init()
    {
        base.Init();
        SetText("Construct");
    }

    protected override void ClickCallback()
    {
        base.ClickCallback();
        _buttonService.ConstructPC(_goods);
    }

    protected void SetText(string text)
    {
        _text.text = text;
    }
}
