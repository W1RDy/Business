public class NotificationController : IService
{
    private NotificationService _notificationService;

    public NotificationController(NotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public void AddNotification(IOrder order)
    {
        var notification = _notificationService.GetNotificationByOrder(order);
        notification.AddNotification();
    }

    public void RemoveNotification(IOrder order)
    {
        var notification = _notificationService.GetNotificationByOrder(order);
        notification.RemoveNotification();
    }
}
