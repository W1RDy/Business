using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CustomImage : ObjectForInitialization, IColorable
{
    private Image _image;
    public MonoBehaviour ColorableObj { get; private set; }
    public Color Color { get => _image.color; set => _image.color = value; }

    public override void Init()
    {
        base.Init();
        _image = GetComponent<Image>();
        ColorableObj = _image;
    }
}
