using Microsoft.AspNetCore.Mvc;
using HotelApp1.Entities.Data;
using HotelApp1.DTO;
using AutoMapper;
using HotelApp1.Entities.Models;
using HotelApp1.Domain.Interface; // Import your entity models or data context as needed

namespace HotelApp1.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class NotificationController : ControllerBase
	{
		private readonly INotificationRepository _notificationRepository;
		private readonly IReservationRepository _reservationRepository;
		private readonly IMapper _mapper;

		public NotificationController(INotificationRepository notificationRepository,
			IReservationRepository reservationRepository,IMapper mapper)
		{
			_notificationRepository = notificationRepository;
			_reservationRepository = reservationRepository;
			_mapper = mapper;
		}

		[HttpGet]
		[ProducesResponseType(200, Type = typeof(IEnumerable<Notification>))]
		public IActionResult GetAllNotifications() {

			var notifications = _mapper.Map<List<NotificationDto>>(_notificationRepository.GetAllNotifications());
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			if (notifications == null )
				return NotFound();
			return Ok(notifications);
		}

		[HttpGet("NotificationId")]
		[ProducesResponseType(200, Type = typeof(Notification))]
		[ProducesResponseType(400)]
		public IActionResult GetNotificationbyId(int notifyid)
		{
			if (!_notificationRepository.DoesNotificationExist(notifyid))
			{
				return NotFound();
			}

			var Client = _mapper.Map<NotificationDto>(_notificationRepository.GetNotification(notifyid));
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);

			}
			return Ok(Client);

		}

		[HttpPost]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		public IActionResult CreateNotification([FromBody] NotificationDto Notificationcreate)
		{
			if (Notificationcreate == null)
			{
				return BadRequest(ModelState);
			}
			
			var Notification = _notificationRepository.GetAllNotifications().Where(s =>
			s.NotificationId == Notificationcreate.NotificationId).FirstOrDefault();
			if (Notification != null)
			{
				ModelState.AddModelError("", "Notification already exists");
				return StatusCode(422, ModelState);
			}
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			if (!_reservationRepository.ReservationExists(Notificationcreate.ReservationId))
			{
				return BadRequest("Reservation does not exist");
			}
			var NotificationMap = _mapper.Map<Notification>(Notificationcreate);

			if (!_notificationRepository.CreateNotification(NotificationMap))
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
		public IActionResult UpdateRoom(int notificationId,
			[FromBody] NotificationDto updatenotification)
		{
			if (updatenotification == null)
			{
				return BadRequest(ModelState);
			}
			if (notificationId != updatenotification.NotificationId)
			{
				return BadRequest(ModelState);
			}
			if (!_notificationRepository.DoesNotificationExist(notificationId))
			{
				return NotFound();
			}
			if (!_reservationRepository.ReservationExists(updatenotification.ReservationId))
			{
				return BadRequest("Reservation not found");
			}
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			var notificationMap = _mapper.Map<Notification>(updatenotification);

			if (!_notificationRepository.UpdateNotification(notificationMap))
			{
				ModelState.AddModelError("", "Somthing went wrong updating notification");
				return StatusCode(500, ModelState);
			}
			return Ok("Succesfully updated notification");
		}

		[HttpDelete("notificationId")]
		[ProducesResponseType(400)]
		[ProducesResponseType(204)]
		[ProducesResponseType(404)]
		public IActionResult Deletenotification(int notificationId)
		{
			if (!_notificationRepository.DoesNotificationExist(notificationId))
			{
				return NotFound();
			}
			var notificationToDelete = _notificationRepository.GetNotification(notificationId);
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			if (!_notificationRepository.DeleteNotification(notificationToDelete))
			{
				ModelState.AddModelError("", "Somthing went wrong deleting notification");
			}
			return NoContent();
		}
	}
}
