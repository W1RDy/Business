public interface IIDGenerator
{
    public int GetID();
    public void BorrowID(int id);
    public void ReleaseID(int id);
}