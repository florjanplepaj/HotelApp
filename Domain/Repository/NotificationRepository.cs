using AutoMapper;
using HotelApp1.Domain.Interface;
using HotelApp1.Entities.Data;
using HotelApp1.Entities.Models;

namespace HotelApp1.Domain.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly HotelAppContext _context;


        public NotificationRepository(HotelAppContext context)
        {
            _context = context;

        }

		public bool CreateNotification(Notification notification)
		{
			_context.Add(notification);
            return Save();
		}

		public bool DeleteNotification(Notification notification)
		{
			_context.Remove(notification);
            return Save();
		}

		public bool DeleteNotifications(List<Notification> notificationList)
		{
			_context.RemoveRange(notificationList);
			return Save();	
		}

		public bool DoesNotificationExist(int id)
        {
            if (_context.Notifications.Any(t => t.NotificationId == id)) return true;
            else return false;
        }

        public ICollection<Notification> GetAllNotifications()
        {
            return _context.Notifications.ToList();
        }

		public ICollection<Notification> GetAllNotificationsbyReservation(int reservationId)
		{
			return _context.Notifications.Where(sl=>sl.ReservationId == reservationId).ToList();
		}

		public Notification GetNotification(int id)
        {
            return _context.Notifications.Where(l => l.NotificationId == id).FirstOrDefault();
        }

		public ICollection<Notification> GetNotificationsbyClientId(int clientId)
		{
			return _context.Notifications.Where(s=>s.ReceiverClientId == clientId || s.SenderClientId == clientId
			).ToList();
		}

		public bool Save()
		{
			var saved = _context.SaveChanges();
			return saved > 0 ? true : false;
		}

		public bool UpdateNotification(Notification notification)
		{
			_context.Update(notification);
            return Save();
		}
	}
}
