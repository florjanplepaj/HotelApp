namespace HotelApp1.DTO
{
	public class NotificationDto
	{
		public int NotificationId { get; set; }
		public int SenderClientId { get; set; }
		public int ReceiverClientId { get; set; }
		public bool Seen { get; set; }
		public int ReservationId { get; set; }
	}
}
