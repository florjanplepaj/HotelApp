using static HotelApp1.DTO.ClientDto;

namespace HotelApp1.Domain.Interface
{
	public interface IClientServices
	{
		Task<Client> RegisterClientAsync(ClientRegistrationDTO request);
		Task<Client> AuthenticateClientAsync(string email, string password);
		Task<Client> RegisterAdminAsync(ClientRegistrationDTO request);

	}
}
