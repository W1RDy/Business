using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class SizeText : MonoBehaviour
{
    private TextMeshProUGUI _text;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _text.rectTransform.sizeDelta = new Vector2(_text.preferredWidth + 10f, _text.rectTransform.sizeDelta.y);
    }
}
