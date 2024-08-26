using AutoMapper;
using HotelApp1.Domain.Interface;
using HotelApp1.Entities.Data;
using HotelApp1.Entities.Models;
using HotelApp1.Helpers;
using static HotelApp1.DTO.ClientDto;

namespace HotelApp1.Domain.Repository
{
	public class ClientRepository : IClientRepository
	{
		private readonly HotelAppContext _context;
		private readonly IMapper _mapper;
		private readonly PasswordService _passwordService;

		public ClientRepository(HotelAppContext context, IMapper mapper, PasswordService passwordService)
		{
			_context = context;
			_mapper = mapper;
			_passwordService = passwordService;
		}

		public bool ClientExist(int clientId)
		{
			return _context.Clients.Any(t => t.ClientId == clientId);
		}

		public bool ClientExist(string clientName)
		{
			return _context.Clients.Any(s => s.Name == clientName);
		}

		public bool CreateClient(Client client)
		{
			_context.Add(client);
			return Save();
		}

		public bool DeleteClient(Client request)
		{
			_context.Remove(request);
			return Save();
		}

		public Client GetClientById(int id)
		{
			return _context.Clients.FirstOrDefault(t => t.ClientId == id);
		}

		public Client GetClientByName(string name)
		{
			return _context.Clients.FirstOrDefault(z => z.Name == name);
		}

		public ICollection<Client> GetClients()
		{
			return _context.Clients.ToList();
		}

		public bool Save()
		{
			return _context.SaveChanges() > 0;
		}

		public bool UpdateClient(ClientRegistrationDTO request)
		{
			var client = _context.Clients.FirstOrDefault(c => c.ClientId == request.ClientId);

			if (client == null)
			{
				throw new InvalidOperationException("Client does not exist.");
			}

			if (_context.Clients.Any(c => c.Username == request.Username && c.ClientId != request.ClientId))
			{
				throw new InvalidOperationException("An account with this username already exists.");
			}

			_mapper.Map(request, client);
			_passwordService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

			client.Username = request.Username;
			client.PasswordHash = passwordHash;
			client.PasswordSalt = passwordSalt;

			_context.Update(client);
			return Save();
		}
	}
}
