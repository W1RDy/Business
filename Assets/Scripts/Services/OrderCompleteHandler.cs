using System;

public class OrderCompleteHandler : IService
{
    private PCService _pcService;

    private Action InitDelegate;

    public OrderCompleteHandler()
    {
        InitDelegate = () =>
        {
            _pcService = ServiceLocator.Instance.Get<PCService>();

            ServiceLocator.Instance.ServiceRegistered -= InitDelegate;
        };
        ServiceLocator.Instance.ServiceRegistered += InitDelegate;
        if (ServiceLocator.Instance.IsRegistered) InitDelegate.Invoke();
    }

    public void CompleteOrder(Order order)
    {
        var pc = _pcService.GetPCByQuality(order.NeededGoods);
        pc.ThrowOut();
    }
}