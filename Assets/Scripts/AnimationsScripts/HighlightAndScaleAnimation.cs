using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu (fileName = "Higligh Animation", menuName = "UIAnimations/New UI Highlight Animation")]
public class HighlightAndScaleAnimation : UIScaleAnimation
{
    [SerializeField] private float _changeValue = 0.2f;
    [SerializeField] private float _segmentDuration = 0.5f;
    [SerializeField] private float _colorSegmentDuration = 0.8f;

    [SerializeField] private Color _endColor;

    [SerializeField] private Material _outlineMaterial;
    private Image _image;
    private float _startScale;

    private Sequence _colorSequence;
    private Color _startOutlineColor;
    private Color _startImageColor;

    public void SetParametres(Transform transform, Image image)
    {
        _transform = transform;
        _image = image;
    }

    public override void Play(Action callback)
    {
        base.Play(callback);
        _image.material = _outlineMaterial;

        _startScale = _transform.localScale.x;
        _startOutlineColor = _outlineMaterial.GetColor("_Color");
        _startImageColor = _image.color;

        _image.color = Color.white;

        _sequence = DOTween.Sequence();
        _colorSequence = DOTween.Sequence();

        _transform.localScale = new Vector3(_startScale - _changeValue, _startScale - _changeValue, 1);

        _sequence
            .Append(_transform.DOScale(_startScale + _changeValue, _segmentDuration * 2))
            .Append(_transform.DOScale(_startScale - _changeValue, _segmentDuration * 2));

        _colorSequence
            .Append(_outlineMaterial.DOColor(_endColor, _colorSegmentDuration))
            .Append(_outlineMaterial.DOColor(_startOutlineColor, _colorSegmentDuration));

        _sequence.SetLoops(-1);
        _colorSequence.SetLoops(-1);
    }

    public override void Kill()
    {
        _colorSequence.Kill();
        base.Kill();
    }

    protected override void Release()
    {
        Debug.Log("ReleaseHiglightValues");
        base.Release();
        _image.material = null;
        _image.color = _startImageColor;
        _outlineMaterial.SetColor("_Color", _startOutlineColor);

        _transform.localScale = new Vector3(_startScale, _startScale, 1);
    }
}