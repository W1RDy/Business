using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public abstract class UIAnimation : ScriptableObject
{
    public bool IsFinished { get; protected set; }

    public abstract void Play();

    public abstract void Play(Action callback);
}
