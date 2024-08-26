using HotelApp1.Entities.Models;

namespace HotelApp1.Domain.Interface
{
    public interface INotificationRepository
    {
        ICollection<Notification> GetAllNotifications();
        Notification GetNotification(int id);
        ICollection<Notification> GetNotificationsbyClientId(int clientId);
        ICollection<Notification> GetAllNotificationsbyReservation(int reservationId); 
        bool CreateNotification(Notification notification); 
        bool DeleteNotification(Notification notification);
        bool DeleteNotifications(List<Notification> notificationList);
        bool UpdateNotification(Notification notification);
        bool DoesNotificationExist(int id);
        bool Save();
	}
}
