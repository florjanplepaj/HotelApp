using System.ComponentModel.DataAnnotations;

namespace HotelApp1.DTO
{
	public class ClientDto
	{
		public class ClientRegistrationDTO
		{

			public int ClientId { get; set; }


			[Required(ErrorMessage = "Name is required")]
			public string Name { get; set; } = string.Empty;

			[Required(ErrorMessage = "Surname is required")]
			public string Surname { get; set; } = string.Empty;
			[Required(ErrorMessage = "Username is required")]
			public string Username { get; set; } = string.Empty;

			[Required(ErrorMessage = "Email is required")]
			public string Email { get; set; } = string.Empty;

			[Required(ErrorMessage = "Password is required")]
			public string Password { get; set; } = string.Empty;

			[Required(ErrorMessage = "Phone number is required")]
			public string PhoneNumber { get; set; } = string.Empty;

			[Required(ErrorMessage = "Birth date is required")]
			public DateTime Birth { get; set; }
			
			

		}


		public class ClientLoginDTO
		{
			[Required(ErrorMessage = "Username is required")]
			public string Username { get; set; } = string.Empty;

			[Required(ErrorMessage = "Password is required")]
			public string Password { get; set; } = string.Empty;

		}


	}
}
