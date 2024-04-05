using TMPro;
using UnityEngine;

public class OrderView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private TextMeshProUGUI _timeText;

    [SerializeField] private ApplyOrderButton _applyOrderButton;
    [SerializeField] private SendOrderButton _sendOrderButton;

    private ButtonChanger _buttonChanger;

    private void Awake()
    {
        _buttonChanger = new ButtonChanger(_applyOrderButton, _sendOrderButton);
    }

    public void SetView(int coinsValue, int timeValue)
    {
        SetCoins(coinsValue);
        SetTime(timeValue);
    }

    public void SetCoins(int coinsValue)
    {
        _coinsText.text = coinsValue.ToString();
    }

    public void SetTime(int timeValue)
    {
        _timeText.text = timeValue.ToString();
    }   

    public void ChangeApplyState(bool isApplied)
    {
        _applyOrderButton.ChangeState(isApplied);
    }

    public void ChangeButton(bool activateDefaultButton)
    {
        _buttonChanger.ChangeButtons(activateDefaultButton);
    }
}
