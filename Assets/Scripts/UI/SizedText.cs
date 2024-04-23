using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class SizedText : MonoBehaviour
{
    private TextMeshProUGUI _text;

    public void SetText(string text)
    {
        if (_text == null) _text = GetComponent<TextMeshProUGUI>();
        _text.text = text;
        _text.rectTransform.sizeDelta = new Vector2(_text.preferredWidth + 10f, _text.rectTransform.sizeDelta.y);
    }
}
