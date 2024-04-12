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

    public virtual void Play()
    {
        Play(null);
    }

    public virtual void Play(Action callback)
    {
        if (!IsFinished) Kill();

        _finishCallback = () =>
        {
            callback?.Invoke();
            _isFinished = true;
        };
    }

    public virtual void Kill()
    {
        _sequence.Kill();
        _finishCallback?.Invoke();
        Release();
    }

    protected virtual void Release()
    {

    }
}
