
using AutoMapper;
using HotelApp1.Domain.Interface;
using HotelApp1.DTO;
using HotelApp1.Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp1.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class RoomController : Controller
	{
		private readonly IRoomRepository _roomRepository;
		private readonly IMapper _mapper;

		public RoomController (IRoomRepository roomRepository, IMapper mapper)
		{
			_roomRepository = roomRepository;
			_mapper = mapper;
		}

		[HttpGet]
		[ProducesResponseType(200 ,Type = typeof(IEnumerable<Room>))]
		public IActionResult GetRooms()
		{
			var room = _mapper.Map<List<RoomDto>>(_roomRepository.GetRooms());
			if(!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			return Ok(room);

		}

		[HttpGet("RoomId")]
		[ProducesResponseType(200, Type = typeof(Room))]
		[ProducesResponseType(400)]
		public IActionResult GetRoom(int id)
		{
			if(!_roomRepository.roomExist(id))
			{
				return NotFound();
			}

			var room = _mapper.Map<RoomDto>(_roomRepository.GetRoom(id));

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);

			}
			return Ok(room);

		}
		//__________________________________________Create_Room_Controller________________________________________

		[HttpPost]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		public IActionResult CreateRoom([FromBody] RoomDto roomcreate)
		{
			if(roomcreate == null)
			{
				return BadRequest(ModelState);
			}

			var room = _roomRepository.GetRooms().Where(s => 
			s.RoomNumber == roomcreate.RoomNumber).FirstOrDefault();
			if ( room != null )
			{
				ModelState.AddModelError("", "Room already exists");
				return StatusCode(422, ModelState);
			}
			if(!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var roomMap = _mapper.Map<Room>(roomcreate);
			
			if (!_roomRepository.CreateRoom(roomMap))
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
		public IActionResult UpdateRoom (int roomId,
			[FromBody] RoomDto updateroom)
		{
			if(updateroom == null)
			{
				return BadRequest(ModelState);	
			}
			if(roomId != updateroom.RoomNumber)
			{
				return BadRequest(ModelState);
			}
			if(!_roomRepository.roomExist(roomId))
			{
				return NotFound();
			}
			if (!ModelState.IsValid)
			{
				return BadRequest();	
			}
			var roomMap = _mapper.Map<Room>(updateroom);

			if (!_roomRepository.UpdateRoom(roomMap))
			{
				ModelState.AddModelError("", "Somthing went wrong updating room");
				return StatusCode(500, ModelState);
			}
			return Ok("Succesfully updated room");
		}

		[HttpDelete("roomId")]
		[ProducesResponseType(400)]
		[ProducesResponseType(204)]
		[ProducesResponseType(404)]
		public IActionResult DeleteRoom (int roomId)
		{
			if (!_roomRepository.roomExist(roomId))
			{
				return NotFound();
			}
			var roomToDelete = _roomRepository.GetRoom(roomId);	
			if(!ModelState.IsValid)
			{
				return BadRequest();
			}
			if (!_roomRepository.DeleteRoom(roomToDelete))
			{
				ModelState.AddModelError("", "Somthing went wrong deleting room");
			}
			return NoContent();
		}


	}
}
