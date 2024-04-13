using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class WindowStackController : MonoBehaviour
{
    private Stack<Window> _windowsStack = new Stack<Window>();
    private int _maxStackSize;
    private int _stackSize = 0;

    public WindowStackController(int maxStackSize)
    {
        _maxStackSize = maxStackSize;
    }

    public void AddWindowToStack(Window window)
    {
        _stackSize = Mathf.Clamp(++_stackSize, 0, _maxStackSize);
        Debug.Log(_stackSize);
        _windowsStack.Push(window);
    }

    public Window PopWindow()
    {
        _stackSize = Mathf.Clamp(--_stackSize, 0, _maxStackSize);
        Debug.Log(_stackSize);
        var window = _windowsStack.Pop();
        if (StackIsEmpty()) ClearStack();

        return window;
    }

    public Window PeekWindow()
    {
        return _windowsStack.Peek();
    }

    public bool StackIsEmpty()
    {
        return _stackSize == 0;
    }

    private void ClearStack()
    {
        Debug.Log("Clear");
        _stackSize = 0;

        if (_windowsStack.Count == 0) return;

        foreach (var window in _windowsStack)
        {
            Debug.Log(window);
            window.DeactivateWindow();
        }
        _windowsStack.Clear();
    }
}
