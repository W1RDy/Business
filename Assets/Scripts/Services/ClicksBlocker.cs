using UnityEngine;
using UnityEngine.UI;

public class ClicksBlocker : MonoBehaviour
{
    [SerializeField] private Image _clickBlocker;

    public void BlockClicks()
    {
        _clickBlocker.enabled = true;
    }

    public void UnblockClicks()
    {
        _clickBlocker.enabled = false;
    }
}
