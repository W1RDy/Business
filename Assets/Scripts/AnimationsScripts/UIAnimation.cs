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

        _finishCallback = () =>
        {
            _isFinished = true;
            Release();

            callback?.Invoke();
            _finishCallback = null;
        };
    }

    public virtual void Kill()
    {
        _sequence.Kill();
        _finishCallback?.Invoke();
    }

    protected virtual void Release()
    {
        IsKillAnimation = false;
    }
}
