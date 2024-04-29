using System;
using System.Collections;
using UnityEngine;

public class ActionInNextFrameActivator : MonoBehaviour, IService
{
    public void ActivateActionInNextFrame(Action action)
    {
        StartCoroutine(ActivateNextFrame(action));
    }

    private IEnumerator ActivateNextFrame(Action action)
    {
        yield return null;
        action?.Invoke();
    }
}