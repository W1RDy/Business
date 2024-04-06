using System.Collections.Generic;
using Random = UnityEngine.Random;

public static class RandomizerWithChances<T> where T : IRandomizable
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

        return result;
    }
}