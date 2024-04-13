using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioButton : CustomButton, IButtonWithStates
{
    [SerializeField] private Image _audioIcon;

    [SerializeField] private Sprite _activatedAudioIcon;
    [SerializeField] private Sprite _deactivatedAudioIcon;

    [SerializeField] private ChangeCondition[] _changeConditions;
    public ChangeCondition[] ChangeConditions => _changeConditions;

    public void ChangeStates(bool toActiveState)
    {
        var icon = toActiveState ? _activatedAudioIcon : _deactivatedAudioIcon;
        _audioIcon.sprite = icon;
    }

    protected override void ClickCallback()
    {
        base.ClickCallback();
        ChangeAudioSettings();
        ChangeStates(CheckStatesChangeCondition());
    }

    private void ChangeAudioSettings()
    {
        _audioPlayer.ChangeAudioSettings(!_audioPlayer.IsPlaying);
    }

    public bool CheckStatesChangeCondition()
    {
        return _audioPlayer.IsPlaying; 
    }
}
