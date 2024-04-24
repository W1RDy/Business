using I2.Loc;
using TMPro;
using UnityEngine;

public abstract class ShowADSForRewardButton : CustomButton
{
    [SerializeField] protected TextMeshProUGUI _buttonText;

    public override void Init()
    {
        base.Init();
    }

    protected override void ClickCallback()
    {
        base.ClickCallback();
        ActivateADS();
    }

    protected virtual void ActivateADS()
    {

    }

    protected virtual void SetText(string messageIndex)
    {
        Debug.Log(_buttonText);
        _buttonText.text = LocalizationManager.GetTranslation(messageIndex);
    }

    public virtual void HideButton()
    {
        gameObject.SetActive(false);
    }

    public virtual void ActivateButton()
    {
        gameObject.SetActive(true);
    }
}
