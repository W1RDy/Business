using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class ADSWarning : MonoBehaviour
{
    [SerializeField] private GameObject _container; 

    public void ActivateWarning()
    {
        _container.SetActive(true);
    }

    public void DeactivateWarning()
    {
        _container.SetActive(false);
    }
}