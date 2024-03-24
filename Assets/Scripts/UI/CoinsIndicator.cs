using TMPro;
using UnityEngine;

public class CoinsIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsText;

    public void SetCoins(int coins)
    {
        _coinsText.text = coins.ToString();
    }
}
