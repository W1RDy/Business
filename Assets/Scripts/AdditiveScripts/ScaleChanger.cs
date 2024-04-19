using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ScaleChanger : MonoBehaviour
{
    [SerializeField] RectTransform[] _transforms;
    [SerializeField] float _scaleChangeValue;

#if UNITY_EDITOR
    [ContextMenu("ChangeScalesWithProportions")]
    public void ChangeScalesWithSavingProportions()
    {
        foreach (var transform in _transforms)
        {
            transform.localScale = transform.localScale * _scaleChangeValue;
            transform.localPosition = transform.localPosition * _scaleChangeValue;
        }
    }
#endif
}
