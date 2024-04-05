public interface IRandomizable<T>
{
    public T Value { get; }
    public float Chance { get; }
    public float ChancesUpdateValue { get; }

    public void UpdateChance(float changeValue);
}