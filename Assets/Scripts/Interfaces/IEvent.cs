using System;

public interface IEvent
{
    public string ID { get; }
    public string Description { get; }
    public event Action Applied;

    public void Apply();
}

public interface IEventWithTimeParameters : IEvent
{
    public int TimeRequirements { get; }

    public void SetTimeParameters(int time);
}

public interface IEventWithCoinsParameters : IEvent
{
    public int CoinsRequirements { get; }

    public void SetCoinsParameters(int coins);
}