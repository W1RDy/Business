using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    [SerializeField] private WindowType _type;

    public WindowType Type => _type;

    public void ActivateWindow()
    {
        gameObject.SetActive(true);
    }

    public void DeactivateWindow()
    {
        gameObject.SetActive(false);
    }
}
