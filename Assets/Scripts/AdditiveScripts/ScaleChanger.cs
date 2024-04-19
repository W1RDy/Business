using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class ScaleChanger : MonoBehaviour
{
    [SerializeField] RectTransform[] _transforms;
    [SerializeField] float _scaleChangeValue;

    private List<RectTransform> _allTransforms = new List<RectTransform>();
    private List<Vector3> _previousScales = new List<Vector3>();
    private List<Vector3> _previousPositons = new List<Vector3>();

#if UNITY_EDITOR
    [ContextMenu("ChangeScalesWithProportions")]
    public void ChangeScalesWithSavingProportions()
    {
        if (_allTransforms.Count == 0)
        {
            foreach (var transform in _transforms)
            {
                var objTransforms = transform.GetComponentsInChildren<RectTransform>();
                _allTransforms.Union(objTransforms);
            }
        }

        Undo.RecordObjects(_allTransforms.ToArray(), "change scale and positions transform");
        foreach (var transform in _allTransforms)
        {
            _previousScales.Add(transform.sizeDelta);
            _previousPositons.Add(transform.localPosition);

            transform.sizeDelta = transform.sizeDelta * _scaleChangeValue;
            transform.localPosition = transform.localPosition * _scaleChangeValue;
        }
    }
#endif

#if UNITY_EDITOR

    [ContextMenu("ReturnScales")]
    public void ReturnChanges()
    {
        if (_previousScales.Count == 0) throw new System.ArgumentOutOfRangeException("You should change scales first!");

        for (int i = 0; i < _previousScales.Count; i++)
        {
            _allTransforms[i].localScale = _previousScales[i];
            _allTransforms[i].localPosition = _previousPositons[i];
        }
    }

#endif
}
