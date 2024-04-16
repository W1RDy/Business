using System;

public class OrderCompleteHandler : ClassForInitialization, IService
{
    private PCService _pcService;

    public OrderCompleteHandler() : base() { }

    public override void Init()
    {
        _pcService = ServiceLocator.Instance.Get<PCService>();
    }

    public void CompleteOrder(Order order)
    {
        var pc = _pcService.GetPCByQuality(order.NeededGoods);
        pc.ThrowOut();
    }
}