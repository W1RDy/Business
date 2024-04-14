using System;
using System.Collections.Generic;
using UnityEngine;

public class RandomController : MonoBehaviour
{
    [SerializeField] private ChancesConfigForView[] _configs;
    private IRandomizable[] _randomizables;

    [SerializeField] private int _minBlocksCount;

    public int MinBlockedCounts => _minBlocksCount;
    public int BlockedCounts { get; private set; }
    public bool IsBlocked { get; private set; }

    private List<IRandomizable> _blockedRandomizables = new List<IRandomizable>();

    public void Init(IRandomizable[] randomizables)
    {
        _randomizables = randomizables;

#if UNITY_EDITOR
        _configs = new ChancesConfigForView[randomizables.Length];

        for (int i = 0; i < _configs.Length; i++)
        {
            _configs[i] = new ChancesConfigForView();
            _configs[i].className = ((ScriptableObject)randomizables[i]).name;
            _configs[i].chance = randomizables[i].Chance;
        }
#endif
    }

    public void UpdateChances(float changeValue, IRandomizable result)
    {
        if (changeValue == 0) return;

        foreach (var randomizable in _randomizables)
        {
            if (!randomizable.Equals(result)) randomizable.UpdateChance(changeValue);
            else result.UpdateChance(-result.ChancesUpdateValue * (_randomizables.Length - 1));

#if UNITY_EDITOR
            UpdateViewChances(randomizable);
#endif
        }
    }

    public void UpdateViewChances(IRandomizable randomizable)
    {
        foreach (var config in _configs)
        {
            if ((randomizable as ScriptableObject).name == config.className)
            {
                config.chance = randomizable.Chance;
                config.isBlocked = randomizable.IsBlocked;
                break;
            }
        }
    }

    public void ChangeChances(float[] chances)
    {
        if (_randomizables.Length != chances.Length) throw new System.ArgumentException("Chances length doesn't equal randomizables length!");

        for (int i = 0; i < _randomizables.Length; i++)
        {
            _randomizables[i].ChangeChance(chances[i]);
            UpdateViewChances(_randomizables[i]);
        }
    }

    public IRandomizable GetRandomizableWithChances()
    {
        if (IsBlocked)
        {
            BlockedCounts++;
            return null;
        }

        var randomizable = RandomizerWithChances<IRandomizable>.Randomize(_randomizables);

        if (randomizable == null)
        {
            UpdateBlockedRandomizables();
            UpdateChances(2, null);
        }
        else
        {
            if (randomizable.IsBlocked) return GetRandomizableWithChances();

            UpdateBlockedRandomizables();
            UpdateChances(randomizable.ChancesUpdateValue, randomizable);
        }

        return randomizable;
    }

    public void BlockController()
    {
        if (!IsBlocked) IsBlocked = true;
    }

    public void UnblockController()
    {
        if (IsBlocked)
        {
            IsBlocked = false;
            BlockedCounts = 0;
        }
    }

    public void BlockRandomizable(IRandomizable randomizable)
    {
        randomizable.IsBlocked = true;
        _blockedRandomizables.Add(randomizable);

#if UNITY_EDITOR

        foreach (var config in _configs)
        {
            if ((randomizable as ScriptableObject).name == config.className)
            {
                config.isBlocked = true;
                break;
            }
        }

#endif

        if (_randomizables.Length == _blockedRandomizables.Count) throw new ArgumentOutOfRangeException("All randomizables are blocked!");
    }

    public void UpdateBlockedRandomizables()
    {
        var listCopy = new List<IRandomizable>(_blockedRandomizables);
        foreach (var randomizable in listCopy)
        {
            randomizable.BlockedCounts++;
            if (randomizable.BlockedCounts >= MinBlockedCounts)
            {
                randomizable.IsBlocked = false;
                randomizable.BlockedCounts = 0;
                _blockedRandomizables.Remove(randomizable);
            }
        }
    }

    public void ChangeMinBlocksCount(int minBlocksCount)
    {
        _minBlocksCount = minBlocksCount;
    }
}

[Serializable]
public class ChancesConfigForView
{
    public string className;
    public float chance;
    public bool isBlocked;
}
