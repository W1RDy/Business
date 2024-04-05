public interface ICoinsCounter
{
    public int Coins { get; }

    public void AddCoins(int value);
    public void RemoveCoins(int value);
}
