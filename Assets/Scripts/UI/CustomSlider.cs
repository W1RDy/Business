using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class CustomSlider : MonoBehaviour
{
    protected Slider _slider;

    protected float _oldValue;

    protected virtual void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        _slider = GetComponent<Slider>();

        _slider.onValueChanged.AddListener(OnValueChangedCallback);
        _oldValue = _slider.value;
    }

    protected virtual void OnValueChangedCallback(float value)
    {
        _oldValue = value;
    }

    private void OnDestroy()
    {
        if (_slider != null)
        {
            _slider.onValueChanged.RemoveListener(OnValueChangedCallback);
        }
    }
}
