using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class FontsChanger : MonoBehaviour
{
    [SerializeField] private GameObject[] _fontsObjs;
    [SerializeField] private TMP_FontAsset _newFont;

    private List<TextMeshProUGUI> _texts = new List<TextMeshProUGUI>();

#if UNITY_EDITOR
    [ContextMenu("Change Fonts")]
    public void ChangeFonts()
    {
        _texts.Clear();
        foreach (var fontObj  in _fontsObjs)
        {
            if (fontObj.TryGetComponent<TextMeshProUGUI>(out var text)) _texts.Add(text);
        }

        //Undo.RecordObjects(_texts.ToArray(), "Change fonts");
        foreach (var text in _texts)
        {
            text.font = _newFont;
        }
    }
#endif
}
