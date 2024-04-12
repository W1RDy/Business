using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour, IService
{
    [SerializeField] private string _defaultMusicIndex;

    private AudioService _audioService;
    private AudioSource _audioSource;

    public bool IsPlaying { get; private set; }

    public void Init(AudioService audioService)
    {
        _audioService = audioService;
        _audioSource = GetComponent<AudioSource>();


        PlayAudio(_defaultMusicIndex);
    }

    public void PlayAudio(string index)
    {
        var audio = _audioService.GetAudioConfig(index);
        if (!IsPlaying || audio.AudioClip != _audioSource.clip)
        {
            StopAudio();
            _audioSource.volume = audio.Volume;
            _audioSource.clip = audio.AudioClip;

            _audioSource.Play();
            IsPlaying = true;
        }
    }

    public void StopAudio()
    {
        if (IsPlaying)
        {
            _audioSource.Stop();
            IsPlaying = false;
        }
    }

    public void ChangeAudioSettings(bool isPlaying)
    {
        if (isPlaying) PlayAudio(_defaultMusicIndex);
        else StopAudio();
    }

    public void PlaySound(string index)
    {
        if (IsPlaying)
        {
            var audio = _audioService.GetAudioConfig(index);

            _audioSource.PlayOneShot(audio.AudioClip, audio.Volume);
        }
    }
}
