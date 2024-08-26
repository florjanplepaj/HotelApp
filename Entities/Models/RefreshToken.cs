using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelApp1.Entities.Models
{
	public class RefreshToken
	{
		
			[Key]
			public int TokenId { get; set; }
			public string Token { get; set; } = string.Empty;
			public DateTime Created { get; set; } = DateTime.Now;
			public DateTime Expires { get; set; }

			[ForeignKey("Client")]
			public int ClientId { get; set; }
			public Client? Client { get; set; }
		
	}
}
