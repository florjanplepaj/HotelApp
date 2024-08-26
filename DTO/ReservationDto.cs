namespace HotelApp1.DTO
{
	public class ReservationDto
	{
		public int ReservationId { get; set; }
		public string ReservationStatus { get; set; } = null!;
		public DateTime CheckInDate { get; set; }
		public DateTime CheckOutDate { get; set; }
		public double TotalPrice { get; set; }
		public int ClientId { get; set; }
		public int RoomNumber { get; set; }
	}
}
