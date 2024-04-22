using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class IDGenerator : IIDGenerator
{
    private int _id = 0;
    private int _maxCount;

    public IDGenerator(int maxCount)
    {
        _maxCount = maxCount;
    }

    public int GetID()
    {
        var id = Mathf.Clamp(_id++, 0, _maxCount);
        if (id == _maxCount) ReleaseID(0);
        return id;
    }

    public void BorrowID(int id)
    {
        _id = id;
    }

    public void ReleaseID(int id)
    {
        Debug.Log("ReleaseID");
        _id = id;
    }
}

public class IDGeneratorWithMinID : IIDGenerator
{
    private bool[] _ids;
    private int _startID;

    public IDGeneratorWithMinID(int maxCount, int startID)
    {
        _startID = startID;
        _ids = new bool[maxCount];

        for (int i = 0; i < _ids.Length; i++)
        {
            _ids[i] = true;
        }
    }

    public int GetID()
    {
        for (int i = 0; i < _ids.Length; i++) 
        {
            if (_ids[i])
            {
                _ids[i] = false;
                return _startID + i;
            }
        }

        throw new System.NullReferenceException("Id hasn't found! Need to increase generator's max count!");
    }

    public void BorrowID(int id)
    {
        _ids[id - _startID] = false;
    }

    public void ReleaseID(int id)
    {
        _ids[id - _startID] = true;
    }
}
