using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CustomText : MonoBehaviour, IColorable
{
    private TextMeshProUGUI _text;

    public MonoBehaviour ColorableObj { get; private set; }
    public Color Color { get => _text.color; set => _text.color = value; }
    public string Text { get => _text.text; set => _text.text = value; }

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        ColorableObj = _text;
    }
}