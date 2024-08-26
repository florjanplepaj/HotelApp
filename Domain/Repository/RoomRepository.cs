using HotelApp1.Domain.Interface;
using HotelApp1.Entities.Data;
using HotelApp1.Entities.Models;
using System.Linq;

namespace HotelApp1.Domain.Repository
{
	public class RoomRepository : IRoomRepository
	{
		private readonly HotelAppContext _context;

		public RoomRepository(HotelAppContext context)
		{
			_context = context;
		}

		public bool CreateRoom(Room room)
		{
			int lastRoomNumber = _context.Rooms.Max(t => t.RoomNumber);
			if (lastRoomNumber ==null ) { lastRoomNumber = 0; };

			room.RoomNumber = lastRoomNumber + 1;

			var roomEntity = new Room
			{
				TypeId = room.TypeId,
				HotelId = 1,
				RoomNumber = room.RoomNumber,
				Availability = "Created"
			};
			_context.Add(roomEntity);
			return Save();
		}

		public bool DeleteRoom(Room room)
		{
			_context.Remove(room);
			return Save();
		}

		public Room GetRoom(int roomId)
		{
			return _context.Rooms.Where(p => p.RoomNumber == roomId).FirstOrDefault();
		}

		public ICollection<Room> GetRooms()
		{
			return _context.Rooms.ToList();
		}

		public bool roomExist(int roomId)
		{
			return _context.Rooms.Any(t => t.RoomNumber == roomId);
		}

		public bool Save()
		{
			var saved = _context.SaveChanges();
			return saved > 0;
		}

		public bool UpdateRoom(Room room)
		{
			_context.Update(room);
			return Save();
		}
	}
}
