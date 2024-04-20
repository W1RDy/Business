using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public abstract class UIAnimation : ScriptableObject
{
    protected Sequence _sequence;

    protected Action _finishCallback;

    protected bool _isFinished = true;
    public bool IsFinished => _isFinished;

    public bool IsKillAnimation { get; set; }

    public virtual void Play()
    {
        Play(null);
    }

    public virtual void Play(Action callback)
    {
        if (!IsFinished || IsKillAnimation) Kill();
        _isFinished = false;

        _finishCallback = () =>
        {
            _finishCallback = null;
            callback?.Invoke();
        };
    }

    protected virtual void Finish()
    {
        if (!_isFinished)
        {
            Release();
            _finishCallback?.Invoke();
            _isFinished = true;
        }
    }

    public virtual void Kill()
    {
        _sequence.Kill();
        Finish();
    }

    protected virtual void Release()
    {
        IsKillAnimation = false;
    }
}
