using I2.Loc;
using TMPro;
using UnityEngine;

public class ConstructPCButton : TutorialButton
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

    protected void SetText(string key)
    {
        _text.text = LocalizationManager.GetTranslation(key);
    }
}
