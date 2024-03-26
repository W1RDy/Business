using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowService : IService
{
    private Dictionary<WindowType, Window> _windowsDict = new Dictionary<WindowType, Window>();

    public WindowService(Window[] windows)
    {
        InitDictionary(windows);
    }

    private void InitDictionary(Window[] windows)
    {
        foreach (Window window in windows)
        {
            _windowsDict.Add(window.Type, window);
        }
    }

    public Window GetWindow(WindowType windowType)
    {
        return _windowsDict[windowType];
    }
}

public enum WindowType
{
    PeriodFinish,
    OrdersWindow
}