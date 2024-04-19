using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using TMPro;

public class ScaleChanger : MonoBehaviour
{
    [SerializeField] RectTransform[] _transforms;
    [SerializeField] RectTransform[] _entitiesTransforms;
    private TextMeshProUGUI[] _transformsTexts;
    private TextMeshProUGUI[] _entitiesTexts;

    [SerializeField] float _scaleChangeValue;
    [SerializeField] float _scaleButtonValue;
    [SerializeField] float _scaleFontsValue;
    [SerializeField] float _scaleEntitiesValue;

    private RectTransform[] _allTransforms;
    private RectTransform[] _allEntitiesTransforms;

#if UNITY_EDITOR
    [ContextMenu("ChangeScalesWithProportions")]
    public void ChangeScalesWithSavingProportions()
    {
        if (_allTransforms != null) _allTransforms = InitTransforms(_transforms, ref _transformsTexts);
        if (_allEntitiesTransforms != null) _allEntitiesTransforms = InitTransforms(_entitiesTransforms, ref _entitiesTexts);

        Undo.RecordObjects(_allTransforms.ToArray(), "change scale and positions transform");
        Undo.RecordObjects(_transformsTexts, "change transform texts");

        foreach (var transform in _allTransforms)
        {
            ChangeParametres(transform, _scaleButtonValue, _scaleFontsValue, _scaleChangeValue);
        }

        Undo.RecordObjects(_entitiesTransforms.ToArray(), "change scale and positions entities transform");
        Undo.RecordObjects(_entitiesTexts, "change entities texts");

        var scaleButtonForEntities = _scaleEntitiesValue / (_scaleChangeValue / _scaleButtonValue);
        var scaleFontsForEntities = _scaleEntitiesValue / (_scaleChangeValue / _scaleFontsValue);

        foreach (var transform in _entitiesTransforms)
        {
            ChangeParametres(transform, scaleButtonForEntities, scaleFontsForEntities, _scaleEntitiesValue);
        }
    }
#endif

    private RectTransform[] InitTransforms(RectTransform[] transforms, ref TextMeshProUGUI[] _texts)
    {
        var list = new List<RectTransform>();
        var textsList = new List<TextMeshProUGUI>();
        foreach (var transform in transforms)
        {
            var objTransforms = transform.GetComponentsInChildren<RectTransform>();
            textsList.AddRange(transform.GetComponentsInChildren<TextMeshProUGUI>());
            list.AddRange(objTransforms);
        }
        _texts = textsList.ToArray();
        return list.ToArray();
    }

    private void ChangeParametres(RectTransform transform, float buttonScaleValue, float fontsScaleValue, float scaleValue)
    {
        var oldClosestBorderPoint = CalculateClosestBorderPoint(transform);

        if (transform.GetComponent<CustomButton>()) transform.sizeDelta = transform.sizeDelta * buttonScaleValue;
        else transform.sizeDelta = transform.sizeDelta * scaleValue;

        if (transform.TryGetComponent<TextMeshProUGUI>(out var text)) text.fontSize = text.fontSize * fontsScaleValue;

        transform.anchoredPosition = CalculateNewPosition(oldClosestBorderPoint, transform);
    }

    private Vector2 CalculateClosestBorderPoint(RectTransform transform)
    {
        Vector2 closestPoint = Vector2.zero;
        if (transform.anchoredPosition.x > 0) closestPoint.x = transform.offsetMin.x;
        else if (transform.anchoredPosition.x < 0) closestPoint.x = transform.offsetMax.x;

        if (transform.anchoredPosition.y > 0) closestPoint.y = transform.offsetMin.y;
        else if (transform.anchoredPosition.y < 0) closestPoint.y = transform.offsetMax.y;

        if (transform.name.StartsWith("Close"))Debug.Log(closestPoint);
        return closestPoint;
    }

    private Vector2 CalculateVectorFromBorderToCenter(RectTransform transform)
    {
        var closestBorderPoint = CalculateClosestBorderPoint(transform);
        var vectorOffset = transform.anchoredPosition - closestBorderPoint;

        return vectorOffset;
    }

    private Vector2 CalculateNewPosition(Vector2 oldClosestBorderPoint, RectTransform transform)
    {
        if (transform.anchorMin.x == 0.5f && transform.anchorMax.x == 0.5f) return transform.anchoredPosition;
        var vectorFromBorderToCenter = CalculateVectorFromBorderToCenter(transform);
        return oldClosestBorderPoint + vectorFromBorderToCenter;
    }
}
