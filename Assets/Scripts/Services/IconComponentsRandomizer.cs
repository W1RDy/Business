using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class IconComponentsRandomizer : MonoBehaviour, IService
{
    [SerializeField] private IconComponents _iconComponents;

    public Sprite RandomizeComponents()
    {
        return GetRandomHair();
    }

    private Sprite GetRandomHair()
    {
        int randomIndex = Random.Range(0, _iconComponents.Hairs.Length);

        return _iconComponents.Hairs[randomIndex];
    }
}
