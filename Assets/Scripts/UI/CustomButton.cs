using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class CustomButton : MonoBehaviour
{
    protected Button _button;

    protected ButtonService _buttonService;

    protected virtual void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        _buttonService = ServiceLocator.Instance.Get<ButtonService>();

        _button = GetComponent<Button>();
        _button.onClick.AddListener(ClickCallback);
    }

    protected virtual void ClickCallback() { }

    private void OnDestroy()
    {
        if (_button != null)
        {
            _button.onClick.RemoveListener(ClickCallback);
        }
    }
}
