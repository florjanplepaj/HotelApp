using AutoMapper;
using HotelApp1.DTO;
using HotelApp1.Domain.Repository;
using Microsoft.AspNetCore.Mvc;
using HotelApp1.Entities.Models;
using HotelApp1.Domain.Interface;

namespace HotelApp1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReservationServiceController : Controller
	{
		private readonly IReservationServicesReporsitory _reservationServicesReporsitory;
		private readonly IReservationRepository _reservationRepository;
		private readonly IMapper _mapper;

		public ReservationServiceController(IReservationServicesReporsitory reservationServicesReporsitory,
			IReservationRepository reservationRepository,IMapper mapper)
		{
			_reservationServicesReporsitory = reservationServicesReporsitory;
			_reservationRepository = reservationRepository;
			_mapper = mapper;
		}
		[HttpGet]
		[ProducesResponseType(200, Type = typeof(IEnumerable<ReservationService>))]
		public IActionResult GetallReservationServices()
		{
			var reservationService = _mapper.Map<List<ReservationServicesDto>>(_reservationServicesReporsitory.GetReservationServices());

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			return Ok(reservationService);
		}


		[HttpGet("ReservationServiceId")]
		[ProducesResponseType(200, Type = typeof(ReservationService))]
		[ProducesResponseType(400)]
		public IActionResult GetClientbyId(int reservationServiceid)
		{
			if (!_reservationServicesReporsitory.ResevationSeviceExists(reservationServiceid))
			{
				return NotFound();
			}

			var reservationService = _mapper.Map<ReservationServicesDto>
				(_reservationServicesReporsitory.GetReservationService(reservationServiceid));
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);

			}
			return Ok(reservationService);

		}

		[HttpPost]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		public IActionResult CreateRoom([FromBody] ReservationServicesDto reservationServicecreate)
		{
			if (reservationServicecreate == null)
			{
				return BadRequest(ModelState);
			}

			var reservationService = _reservationServicesReporsitory.GetReservationServices().Where(s =>
			s.ReservationServicesId == reservationServicecreate.ReservationServicesId).FirstOrDefault();
			if (reservationService != null)
			{
				ModelState.AddModelError("", "ReservationService already exists");
				return StatusCode(422, ModelState);
			}
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var reservationServiceMap = _mapper.Map<ReservationService>(reservationServicecreate);

			if (!_reservationServicesReporsitory.CreateReservationService(reservationServiceMap))
			{
				ModelState.AddModelError("", "Somthing went wrong while saving");
				return StatusCode(500, ModelState);
			}

			return Ok("Succesfully created!");

		}
		[HttpPut]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public IActionResult UpdateRoom(int reservationserviceId,
			[FromBody] ReservationServicesDto updatereservationservice)
		{
			if (reservationserviceId == null)
			{
				return BadRequest(ModelState);
			}
			if (reservationserviceId != updatereservationservice.ReservationServicesId)
			{
				return BadRequest(ModelState);
			}
			if (!_reservationRepository.ReservationExists(updatereservationservice.ReservationId))
			{
				return BadRequest("Reservation not found");
			}
			if (!_reservationServicesReporsitory.ResevationSeviceExists(reservationserviceId))
			{
				return NotFound();
			}
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			var reservationserviceMap = _mapper.Map<ReservationService>(updatereservationservice);

			if (!_reservationServicesReporsitory.UpdateReservationService(reservationserviceMap))
			{
				ModelState.AddModelError("", "Somthing went wrong updating reservationservice");
				return StatusCode(500, ModelState);
			}
			return Ok("Succesfully updated reservationservice");
		}
		[HttpDelete("reservationserviceId")]
		[ProducesResponseType(400)]
		[ProducesResponseType(204)]
		[ProducesResponseType(404)]
		public IActionResult Deletereservationservice(int reservationserviceId)
		{
			if (!_reservationServicesReporsitory.ResevationSeviceExists(reservationserviceId))
			{
				return NotFound();
			}
			var reservationserviceToDelete = _reservationServicesReporsitory.GetReservationService(reservationserviceId);
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			if (!_reservationServicesReporsitory.DeleteReservationService(reservationserviceToDelete))
			{
				ModelState.AddModelError("", "Somthing went wrong deleting reservationservice");
			}
			return NoContent();
		}

	}
}
