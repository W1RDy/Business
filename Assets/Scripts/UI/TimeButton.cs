using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TimeButton : MonoBehaviour
{
    [SerializeField] private int _timeValue;

    [SerializeField] private TextMeshProUGUI _buttonText;
    private Button _button;

    private ButtonService _buttonSertvice;

    private void Start()
    {
        Init();
    }

    private void Init() 
    {
        _buttonSertvice = ServiceLocator.Instance.Get<ButtonService>();

        _button = GetComponent<Button>();
        _button.onClick.AddListener(AddTime);

        SetText();
    }

    public void AddTime()
    {
        _buttonSertvice.AddTime(_timeValue);
    }

    private void SetText()
    {
        _buttonText.text = "Add " + _timeValue + " days";
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(AddTime);
    }
}
