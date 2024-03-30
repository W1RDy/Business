public class IDGenerator
{
    private int _previousID = 0;

    public int GetID()
    {
        return _previousID++;
    }
}
