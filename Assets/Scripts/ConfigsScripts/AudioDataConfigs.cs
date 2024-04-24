using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioDataConfigs", menuName = "GameConfigs/new Audio Data Configs")]
public class AudioDataConfigs : ScriptableObject
{
    [SerializeField] private AudioConfig[] _audioConfigs;

    public AudioConfig[] AudioConfigs => _audioConfigs;
}

[Serializable]
public class AudioConfig
{
    [SerializeField] private string _index;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private float _volume;

    public string Index => _index;
    public AudioClip AudioClip => _audioClip;
    public float Volume => _volume;
}
