using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class CustomButton : ObjectForInitialization
{
    protected Button _button;

    protected ButtonService _buttonService;
    protected AudioPlayer _audioPlayer;

    [SerializeField] private UIAnimation _animation;

    public override void Init()
    {
        base.Init();
        _buttonService = ServiceLocator.Instance.Get<ButtonService>();

        _button = GetComponent<Button>();
        _button.onClick.AddListener(ClickCallback);

        if (_animation)
        {
            _animation = Instantiate(_animation);
            if (_animation is UIScaleAnimation _scaleAnimation) _scaleAnimation.SetParametres(transform); 
        }
        _audioPlayer = ServiceLocator.Instance.Get<AudioPlayer>();
    }

    protected virtual void ClickCallback()
    {
        if (_animation) _animation.Play();
        _audioPlayer.PlaySound("Click");
    }

    private void OnDestroy()
    {
        if (_button != null)
        {
            _button.onClick.RemoveListener(ClickCallback);
        }
    }

    private void OnDisable()
    {
        if (_animation != null && !_animation.IsFinished) _animation.Kill();
    }
}
