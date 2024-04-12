using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioService
{
    private Dictionary<string, AudioConfig> _audioDictionary = new Dictionary<string, AudioConfig>();

    public AudioService(AudioDataConfigs audioDataConfigs)
    {
        InitDictionary(audioDataConfigs);
    }

    private void InitDictionary(AudioDataConfigs audioDataConfigs)
    {
        foreach (var config in audioDataConfigs.AudioConfigs)
        {
            _audioDictionary.Add(config.Index, config);
        }
    }

    public AudioConfig GetAudioConfig(string index)
    {
        return _audioDictionary[index];
    }
}
