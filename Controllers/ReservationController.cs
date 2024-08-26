using AutoMapper;
using HotelApp1.DTO;
using HotelApp1.Domain.Repository;
using Microsoft.AspNetCore.Mvc;
using HotelApp1.Entities.Models;
using HotelApp1.Domain.Interface;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace HotelApp1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReservationController : ControllerBase
	{
		private readonly IReservationRepository _reservationRepository;
		private readonly IClientRepository _clientRepository;
		private readonly IRoomRepository _roomRepository;
		private readonly IExtraServicesRepository _extraServicesRepository;
		private readonly IMapper _mapper;
		private readonly IDataRepository _dataRepository;
		private readonly IReservationServicesReporsitory _reservationServicesReporsitory;
		private readonly INotificationRepository _notificationRepository;

		public ReservationController(IReservationRepository reservationRepository,
			IClientRepository clientRepository,IRoomRepository roomRepository,IExtraServicesRepository extraServicesRepository, IMapper mapper,
			IDataRepository dataRepository,IReservationServicesReporsitory reservationServicesReporsitory,
			INotificationRepository notificationRepository)
		{
			_reservationRepository = reservationRepository;
			_clientRepository = clientRepository;
			_roomRepository = roomRepository;
			_extraServicesRepository = extraServicesRepository;
			_mapper = mapper;
			_dataRepository = dataRepository;
			_reservationServicesReporsitory = reservationServicesReporsitory;
			_notificationRepository = notificationRepository;
		}

		[HttpGet]
		[ProducesResponseType(200, Type = typeof(IEnumerable<ReservationDto>))]
		public IActionResult GetAllReservations()
		{
			var reservations = _reservationRepository.GetAllReservations();
			var reservationDtos = _mapper.Map<List<ReservationDto>>(reservations);

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			return Ok(reservationDtos);
		}
		[HttpGet("client/{clientId}")]
		[ProducesResponseType(200, Type = typeof(IEnumerable<ReservationDto>))]
		[ProducesResponseType(400)]
		public IActionResult GetAllReservationsByClientId(int clientId)
		{
			var reservations = _reservationRepository.GetReservationsByClientId(clientId);

			if (reservations == null || !reservations.Any())
			{
				return NotFound($"No reservations found for client with ID {clientId}");
			}

			var reservationDtos = _mapper.Map<List<ReservationDto>>(reservations);

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			return Ok(reservationDtos);
		}


		[HttpGet("{reservationId}")]
		[ProducesResponseType(200, Type = typeof(ReservationDto))]
		[ProducesResponseType(404)]
		public IActionResult GetReservationById(int reservationId)
		{
			var reservation = _reservationRepository.GetReservation(reservationId);

			if (reservation == null)
			{
				return NotFound();
			}

			var reservationDto = _mapper.Map<ReservationDto>(reservation);

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			return Ok(reservationDto);
		}

		[HttpPost]
		[ProducesResponseType(201)]
		[ProducesResponseType(400)]
		[Authorize]
		public IActionResult CreateReservation([FromBody] ReservationDto reservationDto, 
			[FromQuery] int roomnumber , [FromQuery] int serviceId)
		{
			if (reservationDto == null)
			{
				return BadRequest("Reservation data is null.");
			}

			// Retrieve client ID from claims
			var clientIdClaim = User.Claims.FirstOrDefault(c => c.Type == "ClientId");

			if (clientIdClaim == null)
			{
				return Unauthorized("Client ID not found in token");
			}

			int clientId = int.Parse(clientIdClaim.Value);

			
				var existingReservation = _reservationRepository.GetAllReservations()
		   .FirstOrDefault(r => r.RoomNumber == reservationDto.RoomNumber &&
								r.CheckInDate == reservationDto.CheckInDate &&
								r.CheckOutDate == reservationDto.CheckOutDate &&
								r.ClientId == reservationDto.ClientId);

			
			if (existingReservation != null)
			{
				
				ModelState.AddModelError("", "Reservation already exists with the same details.");
				return StatusCode(422, ModelState);
			}
			// Check room availability
			if (!_reservationRepository.IsRoomAvailable(roomnumber, reservationDto.CheckInDate, reservationDto.CheckOutDate))
			{
				return BadRequest("Room is not available for the selected dates.");
			}

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var reservation = _mapper.Map<Reservation>(reservationDto);
			

			if (!_reservationRepository.CreateReservation(reservation, clientId, roomnumber, serviceId))
			{
				ModelState.AddModelError("", "Something went wrong while saving");
				return StatusCode(500, ModelState);
			}

			
		
			/*var registationRec = new BrowsingData
			{
				ClientId = clientId,
				ActionType = "Useri me id: "+ clientId + " ka kerkuar nje rezervim.",
				Time = DateTime.Now,
			};
			_dataRepository.datacreation(registationRec);

			*/
			return StatusCode(201, "Reservation successfully created");
		}


		[HttpPut]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public IActionResult UpdateReservation([FromQuery] int clientId, [FromQuery] int roomId
			, [FromQuery] int serviceid,[FromQuery] int reservationId,
			[FromBody] ReservationDto updatereservation)
		{
			if (updatereservation == null)
			{
				return BadRequest(ModelState);
			}
			if (reservationId != updatereservation.ReservationId)
			{
				return BadRequest(ModelState);
			}
			if (!_reservationRepository.ReservationExists(reservationId))
			{
				return NotFound();
			}
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			var roomMap = _mapper.Map<Reservation>(updatereservation);

			if (!_reservationRepository.UpdateReservation(roomMap, clientId, reservationId,roomId,serviceid))
			{
				ModelState.AddModelError("", "Somthing went wrong updating reservation");
				return StatusCode(500, ModelState);
			}
			return Ok("Succesfully updated reservation");
		}
		[HttpDelete("reservationId")]
		[ProducesResponseType(400)]
		[ProducesResponseType(204)]
		[ProducesResponseType(404)]
		public IActionResult Deletreservation(int reservationId)
		{
			if (!_reservationRepository.ReservationExists(reservationId))
			{
				return NotFound();
			}

			var reservationservicestoDelete = _reservationServicesReporsitory
				.GetAllReservationServicesbyReservation(reservationId);
			var reservationToDelete = _reservationRepository.GetReservation(reservationId);

			var notificationToDelete = _notificationRepository.GetAllNotificationsbyReservation(reservationId);
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			if (!_reservationServicesReporsitory.DeleteReservationServices(reservationservicestoDelete.ToList())){
				ModelState.AddModelError("", "Somthing weent wrong deleting reservationservices");
			}
			if (!_notificationRepository.DeleteNotifications(notificationToDelete.ToList()))
			{
				ModelState.AddModelError("", "Somthing weent wrong deleting notifications");

			}
			if (!_reservationRepository.DeleteReservation(reservationToDelete))
			{
				ModelState.AddModelError("", "Somthing went wrong deleting reservation");
			}
			return NoContent();
		}
	}
}
