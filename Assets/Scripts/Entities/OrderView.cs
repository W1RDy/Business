using TMPro;
using UnityEngine;

public class OrderView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private TextMeshProUGUI _timeText;

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
}
