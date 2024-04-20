using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBlockWithStates : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _text;

    [SerializeField] private Color _activeColor;
    [SerializeField] private Color _inactiveColor;

    public void ChangeState(bool toActiveState)
    {
        var color = toActiveState ? _activeColor : _inactiveColor;
        _image.color = color;
        _text.color = color;
    }
}