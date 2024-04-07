public interface IRandomizable
{
    public float Chance { get; }
    public float ChancesUpdateValue { get; }
    public bool IsBlocked { get; set; }
    public int BlockedCounts { get; set; }

    public void UpdateChance(float changeValue);
}
