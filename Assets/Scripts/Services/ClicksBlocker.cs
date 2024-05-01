using UnityEngine;
using UnityEngine.UI;

public class ClicksBlocker : MonoBehaviour, IService
{
    [SerializeField] private Image _clickBlocker;
    private int _blocksCount = 1;

    public bool IsBlocked => _blocksCount > 0;

    public void BlockClicks()
    {
        _blocksCount++;
        _clickBlocker.enabled = true;
    }

    public void UnblockClicks()
    {
        _blocksCount--;
        if (_blocksCount <= 0)
        {
            _blocksCount = 0;
            _clickBlocker.enabled = false;
        }
    }
}
