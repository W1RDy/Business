using System.Collections.Generic;
using Random = UnityEngine.Random;

public static class RandomizerWithChances<T> where T : IRandomizable<T> 
{
    public static T Randomize(T[] randomizables)
    {
        var chance = Random.Range(1, 101);
        var chancesSum = 1f;
        var result = default(T);
        foreach (var randomizable in randomizables)
        {
            chancesSum += randomizable.Chance;
            if (chancesSum >= chance)
            {
                result = randomizable;
                break;
            }
        }

        UpdateChances(result.ChancesUpdateValue, result, randomizables);
        return result;
    }

    private static void UpdateChances(float changeValue, T result, T[] randomizables)
    {
        foreach (var randomizable in randomizables)
        {
            if (!randomizable.Equals(result)) randomizable.UpdateChance(changeValue);
            else result.UpdateChance(-result.ChancesUpdateValue * (randomizables.Length - 1));
        }
    }
}