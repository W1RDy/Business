using TMPro;
using UnityEngine;

public class OrderView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private TextMeshProUGUI _timeText;

    [SerializeField] private ApplyOrderButton _applyOrderButton;
    [SerializeField] private SendOrderButton _sendOrderButton;

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

    public void ChangeViewForComplete()
    {
        _applyOrderButton.gameObject.SetActive(false);
        _sendOrderButton.gameObject.SetActive(true);
    }

    public void ChangeViewForProcess()
    {
        _applyOrderButton.gameObject.SetActive(true);
        _sendOrderButton.gameObject.SetActive(false);
    }
}
