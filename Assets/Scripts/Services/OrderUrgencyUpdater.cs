public class OrderUrgencyUpdater
{
    private OrderService _orderService;

    public OrderUrgencyUpdater()
    {
        _orderService = ServiceLocator.Instance.Get<OrderService>();
    }

    public void UpdateUrgency()
    {
        foreach (Order order in _orderService.GetOrders())
        {
            order.UpdateOrderUrgency();
        }
    }
}
