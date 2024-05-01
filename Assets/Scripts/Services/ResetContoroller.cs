using System;
using System.Collections.Generic;
using UnityEngine;

public class ResetContoroller : IService
{
    private List<IResetable> _resetables = new List<IResetable>();

    public bool IsReseted { get; private set; }

    public void AddResetable(IResetable resetable)
    {
        _resetables.Add(resetable);
    }

    public void Reset()
    {
        IsReseted = false;
        foreach (var resetable in _resetables)
        {
            resetable.Reset();
        }
        IsReseted = true;
    }
}