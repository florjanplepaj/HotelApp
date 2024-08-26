using AutoMapper;
using HotelApp1.Domain.Interface;
using HotelApp1.Domain.Repository;
using HotelApp1.DTO;
using HotelApp1.Entities.Data;
using HotelApp1.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using static HotelApp1.DTO.ClientDto;

namespace HotelApp1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		public static Client client = new Client();
		private readonly HotelAppContext _context;
		private readonly IMapper _mapper;
		private readonly IClientServices _clientService;
		private readonly TokenService _tokenService;
		private readonly IDataRepository _dataRepository;

		public AuthController(HotelAppContext context,IMapper mapper, 
			IClientServices clientService, TokenService tokenService,
			IDataRepository dataRepository)
		{
			_context = context;
			_mapper = mapper;
			_clientService = clientService;
			_tokenService = tokenService;
		    _dataRepository = dataRepository;
		}
//____________________________________________________Register User___________________________________________________________________________

		[HttpPost("register")]
		public async Task<ActionResult<Client>> Register(ClientRegistrationDTO request)
		{
			var client = await _clientService.RegisterClientAsync(
				_mapper.Map<ClientRegistrationDTO>(request));
			
			return Ok(client);
		}

//____________________________________________________Log In___________________________________________________________________________

		[HttpPost("login")]
		public async Task<ActionResult<string>> Login(ClientLoginDTO request)
		{

			var client = await _clientService.AuthenticateClientAsync(request.Username, request.Password);

			if (client == null)
			{
				return Unauthorized("Invalid email or password.");
			}

			string token = _tokenService.CreateToken(client);


			var newRefreshToken = TokenService.GenerateRefreshToken();


			newRefreshToken.ClientId = client.ClientId;


			_tokenService.SetRefreshToken(HttpContext, client, newRefreshToken);

			var registationRec = new BrowsingData
			{
				ClientId = client.ClientId,
				ActionType = client.Username + " eshte loguar",
				Time = DateTime.Now,
			};
			_dataRepository.datacreation(registationRec);



			return Ok(new
			{
				AccessToken = token,
				RefreshToken = newRefreshToken.Token
			});

		}
//____________________________________________________Register Admin___________________________________________________________________________

		[HttpPost("register-admin")]
		[Authorize(Roles = "Admin")]

		public async Task<ActionResult<Client>> RegisterAdmin(ClientRegistrationDTO request)
		{
			var admin = await _clientService.RegisterAdminAsync(_mapper.Map<ClientRegistrationDTO>(request));
			var registationRec = new BrowsingData
			{
				ClientId = admin.ClientId,
				ActionType = admin.Username + " eshte regjistruar si admin",
				Time = DateTime.Now,
			};
			_dataRepository.datacreation(registationRec);

			return Ok(admin);
		}
//____________________________________________________Refresh Token___________________________________________________________________________
		[HttpPost("refresh-token")]
		public async Task<ActionResult<string>> RefreshToken()
		{
			var refreshToken = Request.Cookies["refreshToken"];

			var refreshTokenEntity = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);

			if (refreshTokenEntity == null)
			{
				return Unauthorized("Invalid Refresh Token.");
			}
			else if (refreshTokenEntity.Expires < DateTime.Now)
			{
				return Unauthorized("Refresh token expired.");
			}

			var client = await _context.Clients.FindAsync(refreshTokenEntity.ClientId);
			if (client == null)
			{
				return Unauthorized("Client not found.");
			}

			string token = _tokenService.CreateToken(client);

			return Ok(token);
		}

		/*
		[HttpPost("register")]
		public async Task<ActionResult<Client>> Register(ClientDto request)
		{
			if(_context.Clients.Any(p=> p .Username == request.Username))
			{
				throw new InvalidOperationException("An account with this username already exists.");
			}
			var client = _mapper.Map<Client>(request);
			_passwordService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

			client.PasswordHash = passwordHash;
			client.PasswordSalt = passwordSalt;
			_context.Clients.Add(client);
			Save();
			return Ok(client);

			
		}*/
		/*[HttpPost("login")]
		public async Task<ActionResult<string>> Login(ClientDto request)
		{
			var client = _context.Clients.FirstOrDefault(s => s.Username == request.Username);
			if (client == null)
			{
				return BadRequest("User not found");
			}
			if (!_passwordService.VerifyPasswordHash(request.Password, client.PasswordHash, client.PasswordSalt))
			{
				return BadRequest("Wrong Password.");

			}
			//string token = CreateToken(client);
			return Ok("my crazy token");


		}*/
		/*
		[HttpPost("login")]
		public async Task<ActionResult<string>> Login(ClientLoginDTO request)
		{

			/*var client = await _clientService.AuthenticateClientAsync(request.Username, request.Password);

			if (client == null)
			{
				return Unauthorized("Invalid username or password.");
			}



			//string token = _tokenService.CreateToken(client);


			//var newRefreshToken = TokenService.GenerateRefreshToken();


			//newRefreshToken.ClientId = client.ClientId;


			//_tokenService.SetRefreshToken(HttpContext, client, newRefreshToken);

			/*return Ok(new
			{
				AccessToken = token,
				RefreshToken = newRefreshToken.Token
			});
			return Ok(client);*/
	}


	

	}
