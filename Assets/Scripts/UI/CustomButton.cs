using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class CustomButton : MonoBehaviour
{
    protected Button _button;

    protected ButtonService _buttonService;

    [SerializeField] private UIAnimation _animation;

    protected virtual void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        _buttonService = ServiceLocator.Instance.Get<ButtonService>();

        _button = GetComponent<Button>();
        _button.onClick.AddListener(ClickCallback);

        if (_animation)
        {
            _animation = Instantiate(_animation);
            if (_animation is UIScaleAnimation _scaleAnimation) _scaleAnimation.SetParametres(transform); 
        }
    }

    protected virtual void ClickCallback()
    {
        if (_animation) _animation.Play();
    }

    private void OnDestroy()
    {
        if (_button != null)
        {
            _button.onClick.RemoveListener(ClickCallback);
        }
    }
}
