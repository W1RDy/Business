using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class CustomButton : MonoBehaviour
{
    protected Button _button;

    protected ButtonService _buttonSertvice;

    private void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        _buttonSertvice = ServiceLocator.Instance.Get<ButtonService>();

        _button = GetComponent<Button>();
        _button.onClick.AddListener(ClickCallback);
    }

    protected virtual void ClickCallback() { }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(ClickCallback);
    }
}