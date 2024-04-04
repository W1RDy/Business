using System;
using UnityEngine;

public class EntityAnimationsController
{
    [SerializeField] private UIAnimation _appearAnimation;
    [SerializeField] private UIAnimation _disappearAnimation;

    private GameObject _entity;

    public EntityAnimationsController(UIAnimation appearAnimation, UIAnimation disappearAnimation, GameObject entity)
    {
        _appearAnimation = appearAnimation;
        _disappearAnimation = disappearAnimation;

        _entity = entity;
        InitAnimations();
    }

    private void InitAnimations()
    {
        _appearAnimation = ScriptableObject.Instantiate(_appearAnimation);
        _disappearAnimation = ScriptableObject.Instantiate(_disappearAnimation);

        if (_appearAnimation is UIScaleAnimation appearScaleAnimation) appearScaleAnimation.SetParametres(_entity.transform);
        if (_disappearAnimation is UIScaleAnimation disappearScaleAnimation) disappearScaleAnimation.SetParametres(_entity.transform);
    }

    public void PlayAppearAnimation()
    {
        if (_entity.activeInHierarchy) _appearAnimation.Play();
    }

    public void PlayDisappearAnimation(Action callback) 
    {
        if (_entity.activeInHierarchy) _disappearAnimation.Play(callback);
        else callback?.Invoke();
    }
}