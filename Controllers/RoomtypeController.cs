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
	public class RoomtypeController : Controller
	{
		private readonly IRoomTypeRepository _roomTypeRepository;
		private readonly IMapper _mapper;

		public RoomtypeController(IRoomTypeRepository roomTypeRepository, IMapper mapper)
		{
			_roomTypeRepository = roomTypeRepository;
			_mapper = mapper;
		}

		[HttpGet]
		[ProducesResponseType(200, Type = typeof(IEnumerable<RoomType>))]
		public IActionResult GetRoomtypes()
		{
			var RoomType = _mapper.Map<List<RoomtypeDto>>(_roomTypeRepository.GetRoomtypes());
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			return Ok(RoomType);

		}

		[HttpGet("RoomTypeId")]
		[ProducesResponseType(200, Type = typeof(RoomType))]
		[ProducesResponseType(400)]
		public IActionResult GetRoomtype(int id)
		{
			if (!_roomTypeRepository.roomtypeexist(id))
			{
				return NotFound();
			}

			var RoomType = _mapper.Map<RoomtypeDto>(_roomTypeRepository.GetRoomtype(id));

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);

			}
			return Ok(RoomType);

		}

		[HttpPost]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		public IActionResult CreateRoom([FromBody] RoomtypeDto roomtypecreate)
		{
			if (roomtypecreate == null)
			{
				return BadRequest(ModelState);
			}

			var room = _roomTypeRepository.GetRoomtypes().Where(s =>
			s.Type == roomtypecreate.Type).FirstOrDefault();
			if (room != null)
			{
				ModelState.AddModelError("", "Roomtype already exists");
				return StatusCode(422, ModelState);
			}
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var roomtypeMap = _mapper.Map<RoomType>(roomtypecreate);

			if (!_roomTypeRepository.CreateRoomType(roomtypeMap))
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
		public IActionResult UpdateRoomtype(int roomtypeId,
			[FromBody] RoomtypeDto updateroomtype)
		{
			if (updateroomtype == null)
			{
				return BadRequest(ModelState);
			}
			if (roomtypeId != updateroomtype.TypeId)
			{
				return BadRequest(ModelState);
			}
			if (!_roomTypeRepository.roomtypeexist(roomtypeId))
			{
				return NotFound();
			}
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			var roomMap = _mapper.Map<RoomType>(updateroomtype);

			if (!_roomTypeRepository.UpdateRoomType(roomMap))
			{
				ModelState.AddModelError("", "Somthing went wrong updating roomtype");
				return StatusCode(500, ModelState);
			}
			return Ok("Succesfully updated");
		}
		[HttpDelete("roomtypeId")]
		[ProducesResponseType(400)]
		[ProducesResponseType(204)]
		[ProducesResponseType(404)]
		public IActionResult Deletereservationservice(int roomtypeId)
		{
			if (!_roomTypeRepository.roomtypeexist(roomtypeId))
			{
				return NotFound();
			}
			var roomtypeToDelete = _roomTypeRepository.GetRoomtype(roomtypeId);
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			if (!_roomTypeRepository.DeleteRoomType(roomtypeToDelete))
			{
				ModelState.AddModelError("", "Somthing went wrong deleting roomtype");
			}
			return NoContent();
		}

	}
}
