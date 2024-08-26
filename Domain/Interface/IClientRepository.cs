using HotelApp1.DTO;
using HotelApp1.Entities.Models;
using static HotelApp1.DTO.ClientDto;

namespace HotelApp1.Domain.Interface
{
    public interface IClientRepository
    {
        ICollection<Client> GetClients();
        Client GetClientById(int id);
        Client GetClientByName(string name);

        bool CreateClient(Client client);
        bool UpdateClient(ClientRegistrationDTO request);
        bool DeleteClient(Client request);

		bool ClientExist(int clientid);
        bool ClientExist(string clientname);
        bool Save();



    }
}
