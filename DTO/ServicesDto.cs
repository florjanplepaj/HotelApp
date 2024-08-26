namespace HotelApp1.DTO
{
	public class ServicesDto
	{
		public int ServicesId { get; set; }
		public string Type { get; set; } = null!;
		public double Price { get; set; }
		public string Description { get; set; } = null!;
	}
}
