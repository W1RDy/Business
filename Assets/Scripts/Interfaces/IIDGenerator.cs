public interface IIDGenerator
{
    public int GetID();
    public void ReleaseID(int id);
}