using Microsoft.AspNetCore.Mvc;
using HotelApp1.Entities.Data;
using HotelApp1.DTO;
using AutoMapper;
using HotelApp1.Entities.Models;
using HotelApp1.Domain.Interface;
using static HotelApp1.DTO.ClientDto;
using Microsoft.AspNetCore.Authorization;
using HotelApp1.Domain.Repository;

namespace HotelApp1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ClientController : ControllerBase
	{
		private readonly IClientRepository _clientRepository;
		private readonly IMapper _mapper;
		private readonly IReservationRepository _reservationRepository;
		private readonly INotificationRepository _notificationRepository;
		private readonly IDataRepository _dataRepository;

		public ClientController(IClientRepository clientRepository, IMapper mapper,
			IReservationRepository reservationRepository,INotificationRepository notificationRepository,
			IDataRepository dataRepository)
		{
			_clientRepository = clientRepository;
			_mapper = mapper;
			_reservationRepository = reservationRepository;
			_notificationRepository = notificationRepository;
			_dataRepository = dataRepository;
		}

		[HttpGet]
		[ProducesResponseType(200, Type = typeof(IEnumerable<ClientRegistrationDTO>))]
		public IActionResult GetClients()
		{
			var clients = _mapper.Map<List<ClientRegistrationDTO>>(_clientRepository.GetClients());
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			return Ok(clients);
		}

		[HttpGet("ClientId/{clientId}")]
		[ProducesResponseType(200, Type = typeof(ClientRegistrationDTO))]
		[ProducesResponseType(400)]
		public IActionResult GetClientById(int clientId)
		{
			if (!_clientRepository.ClientExist(clientId))
			{
				return NotFound();
			}

			var client = _mapper.Map<ClientRegistrationDTO>(_clientRepository.GetClientById(clientId));
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			return Ok(client);
		}

		[HttpGet("ClientName/{clientName}")]
		[ProducesResponseType(200, Type = typeof(ClientRegistrationDTO))]
		[ProducesResponseType(400)]
		public IActionResult GetClientByName(string clientName)
		{
			if (!_clientRepository.ClientExist(clientName))
			{
				return NotFound();
			}

			var client = _mapper.Map<ClientRegistrationDTO>(_clientRepository.GetClientByName(clientName));
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			return Ok(client);
		}

		[HttpPut("{clientId}")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		[Authorize]
		public IActionResult UpdateClient(int clientId, [FromBody] ClientRegistrationDTO updateClient)
		{
			if (updateClient == null)
			{
				return BadRequest(ModelState);
			}

			var clientIdClaim = User.Claims.FirstOrDefault(c => c.Type == "ClientId");

			if (clientIdClaim == null)
			{
				return Unauthorized("Client ID not found in token");
			}

			int clientIdFromToken = int.Parse(clientIdClaim.Value);

			if (clientId != clientIdFromToken)
			{
				return BadRequest(ModelState);
			}

			if (!_clientRepository.ClientExist(clientId))
			{
				return NotFound();
			}

			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			if (!_clientRepository.UpdateClient(updateClient))
			{
				ModelState.AddModelError("", "Something went wrong updating the client");
				return StatusCode(500, ModelState);
			}
			return Ok("Successfully updated client");
		}

		[HttpDelete("{clientId}")]
		[ProducesResponseType(400)]
		[ProducesResponseType(204)]
		[ProducesResponseType(404)]
		public IActionResult DeleteClient(int clientId)
		{
			// Check if the client exists
			if (!_clientRepository.ClientExist(clientId))
			{
				return NotFound();
			}

			// Fetch client and dependent records
			var clientToDelete = _clientRepository.GetClientById(clientId);
			var reservations = _reservationRepository.GetReservationsByClientId(clientId);
			var notifications = _notificationRepository.GetNotificationsbyClientId(clientId); // Fetch notifications where client is either sender or receiver
			var data = _dataRepository.GetDataFromClientId(clientId);

			// Check if there are any reservations
			if (reservations.Any())
			{
				ModelState.AddModelError("", "Before deleting your account, please delete all your reservations.");
				return BadRequest(ModelState);
			}

			// Attempt to delete dependent notifications
			if (notifications.Any())
			{
				if (!_notificationRepository.DeleteNotifications(notifications.ToList()))
				{
					ModelState.AddModelError("", "Something went wrong deleting notifications.");
					return StatusCode(500, ModelState);
				}
			}

			// Attempt to delete dependent data
			var dataToDelete = _dataRepository.GetDataFromClientId(clientId);
			if (dataToDelete != null || dataToDelete.Any())
			{
				if (!_clientRepository.DeleteClient(clientToDelete))
				{
					ModelState.AddModelError("", "Something went wrong deleting the client.");
					return StatusCode(500, ModelState);
				}
			}


			

			return NoContent();
		}


	}
}
