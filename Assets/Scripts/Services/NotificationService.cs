using System.Collections.Generic;

public class NotificationService
{
    private Dictionary<NotificationType, Notification> _notifications = new Dictionary<NotificationType, Notification>();

    public NotificationService(Notification[] notifications)
    {
        InitDictionary(notifications);
    }

    private void InitDictionary(Notification[] notifications)
    {
        foreach(var notification in notifications)
        {
            _notifications.Add(notification.NotificationType, notification);
        }
    }

    public Notification GetNotification(NotificationType type) => _notifications[type];

    public Notification GetNotificationByOrder(IOrder order)
    {
        if (order as Order != null)
        {
            return GetNotification(NotificationType.OrderNotification);
        }
        else if (order as DeliveryOrder != null)
        {
            return GetNotification(NotificationType.DeliveryOrderNotification);
        }
        return null;
    }
}

public enum NotificationType
{
    OrderNotification,
    DeliveryOrderNotification
}
