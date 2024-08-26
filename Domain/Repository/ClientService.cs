using AutoMapper;
using HotelApp1.Domain.Interface;
using HotelApp1.DTO;
using HotelApp1.Entities.Data;
using HotelApp1.Entities.Models;
using HotelApp1.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using static HotelApp1.DTO.ClientDto;

namespace HotelApp1.Domain.Repository
{
	public class ClientService : IClientServices
	{
		private readonly PasswordService _passwordService;
		private readonly HotelAppContext _context;
		private readonly IDataRepository _dataRepository;
		private readonly IMapper _mapper;

		public ClientService( IMapper mapper, PasswordService passwordService, HotelAppContext context,
			IDataRepository dataRepository) 
		{
			_passwordService = passwordService;
			_context = context;
			_dataRepository = dataRepository;
			_mapper = mapper;
		}


		//----------------------------------------------------------------------REGISTER CLIENT-----------------------------------------------------------------------------
		public async Task<Client> RegisterClientAsync(ClientRegistrationDTO request)
		{
			if (_context.Clients.Any(c => c.Username == request.Username))
			{
				throw new InvalidOperationException("An account with this username already exists.");
			}

			var client = _mapper.Map<Client>(request);
			_passwordService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

			client.Username = request.Username;
			client.PasswordHash = passwordHash;
			client.PasswordSalt = passwordSalt;

			_context.Clients.Add(client);
			await _context.SaveChangesAsync();

			/*var registationRec = new BrowsingData
			{
				ClientId = client.ClientId,
				ActionType = client.Username + " eshte regjistruar",
				Time = DateTime.Now,
			};
			_dataRepository.datacreation(registationRec);

			*/

			return client;
		}
		
	

		//---------------------------------------------------------------------------REGISER_ADMIN-----------------------------------------------------------------------------------
		public async Task<Client> RegisterAdminAsync(ClientRegistrationDTO request)
		{
			if (await IsUsernameRegisteredAsync(request.Username))
			{
				throw new Exception("Usename is already registered.");
			}
			

			var admin = _mapper.Map<Client>(request);
			admin.Role = "Admin";
			_passwordService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
			admin.PasswordHash = passwordHash;
			admin.PasswordSalt = passwordSalt;
			

			_context.Clients.Add(admin);
			await _context.SaveChangesAsync();


			return admin;
		}
		private async Task<bool> IsUsernameRegisteredAsync(string username)
		{
			if (_context.Clients.Any(c => c.Username == username))
			{
				throw new InvalidOperationException("An account with this username already exists.");
			}
			return await _context.Clients.AnyAsync(c => c.Username == username);
		}
		//----------------------------------------------------------------------------LOGIN-------------------------------------------------------------------------------------

		public async Task<Client> AuthenticateClientAsync(string username, string password)
		{

			var client = await _context.Clients.FirstOrDefaultAsync(c => c.Username == username);

			if (client == null)
			{
				throw new Exception("Client with the provided username does not exist");
			}


			if (!_passwordService.VerifyPasswordHash(password, client.PasswordHash, client.PasswordSalt))
			{
				throw new Exception("Password doesn't match");
			}

			//client.LastLogin = DateTime.Now;
			await _context.SaveChangesAsync();

			return client;
		}

	}
}
