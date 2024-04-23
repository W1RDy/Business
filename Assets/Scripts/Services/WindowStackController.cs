using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class WindowStackController : ResetableClassForInit
{
    private Stack<Window> _windowsStack = new Stack<Window>();
    private int _maxStackSize;
    private int _stackSize = 0;

    public WindowStackController(int maxStackSize) : base()
    {
        _maxStackSize = maxStackSize;
    }

    public override void Init()
    {
        base.Init();
    }

    public void AddWindowToStack(Window window)
    {
        if (!_windowsStack.Contains(window))
        {
            _stackSize = Mathf.Clamp(++_stackSize, 0, _maxStackSize);
        }
        _windowsStack.Push(window);
    }

    public Window PopWindow()
    {
        if (_windowsStack.Count > 0)
        {
            _stackSize = Mathf.Clamp(--_stackSize, 0, _maxStackSize);
            var window = _windowsStack.Pop();
            if (StackIsEmpty()) ClearStack();

            return window;
        }
        return null;
    }

    public Window PeekWindow()
    {
        return _windowsStack.Peek();
    }

    public bool StackIsEmpty()
    {
        return _stackSize == 0;
    }

    public void ClearStack()
    {
        _stackSize = 0;

        if (_windowsStack.Count == 0) return;

        foreach (var window in _windowsStack)
        {
            window.DeactivateWindow();
        }
        _windowsStack.Clear();
    }

    public override void Reset()
    {
        ClearStack();
    }
}
