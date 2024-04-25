using UnityEngine;
using UnityEngine.UI;
using YG;

[RequireComponent(typeof(Button))]
public abstract class CustomButton : ObjectForInitialization
{
    protected Button _button;

    protected ButtonService _buttonService;
    protected AudioPlayer _audioPlayer;

    [SerializeField] private UIAnimation _animation;

    private TutorialActivator _tutorialActivator;

    private Color _disabledColor;

    public override void Init()
    {
        base.Init();
        _buttonService = ServiceLocator.Instance.Get<ButtonService>();
        _tutorialActivator = ServiceLocator.Instance.Get<TutorialActivator>();

        _button = GetComponent<Button>();
        _disabledColor = _button.colors.disabledColor;

        _tutorialActivator.TutorialActivated += TutorialActivateDelegate;
        _tutorialActivator.TutorialDeactivated += ReturnValuesToDefaultByTutorial;

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

    #region Tutorial

    private void TutorialActivateDelegate()
    {
        InitByTutorial();
        if (YandexGame.savesData.tutorialPartsCompleted == 0) DeactivateClickableByTutorial();
    }

    protected virtual void InitByTutorial()
    {
        if (YandexGame.savesData.tutorialPartsCompleted > 0)
        {
            _tutorialActivator.TutorialActivated -= DeactivateClickableByTutorial;
            _tutorialActivator.TutorialDeactivated -= ReturnValuesToDefaultByTutorial;
        }
    }

    protected virtual void DeactivateClickableByTutorial()
    {
        var colors = _button.colors;
        colors.disabledColor = _button.colors.normalColor;
        _button.colors = colors;
        _button.interactable = false;
    }

    protected virtual void ActivateClickableByTutorial()
    {
        _button.interactable = true;
        var colors = _button.colors;
        colors.disabledColor = _disabledColor;
        _button.colors = colors;
    }

    private void ReturnValuesToDefaultByTutorial()
    {
        ActivateClickableByTutorial();
        _tutorialActivator.TutorialActivated -= DeactivateClickableByTutorial;
        _tutorialActivator.TutorialDeactivated -= ReturnValuesToDefaultByTutorial;
    }

    #endregion 

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (_button != null)
        {
            _button.onClick.RemoveListener(ClickCallback);
            _tutorialActivator.TutorialActivated -= DeactivateClickableByTutorial;
            _tutorialActivator.TutorialDeactivated -= ReturnValuesToDefaultByTutorial;
        }
    }

    private void OnDisable()
    {
        if (_animation != null && !_animation.IsFinished) _animation.Kill();
    }
}
