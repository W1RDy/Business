using UnityEngine;

public class IDGenerator
{
    private int _id = 0;

    public int GetID()
    {
        var id = Mathf.Clamp(_id++, 0, 1000);
        return id;
    }
}
