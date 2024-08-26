using HotelApp1.Domain.Interface;
using HotelApp1.Entities.Data;
using HotelApp1.Entities.Models;

namespace HotelApp1.Domain.Repository
{
    public class RoomtypeRepository : IRoomTypeRepository
    {
        private readonly HotelAppContext _context;

        public RoomtypeRepository(HotelAppContext context)
        {
            _context = context;
        }

		public bool CreateRoomType(RoomType roomType)
		{
            _context.Add(roomType);
            return Save();
		}

		public bool DeleteRoomType(RoomType roomType)
		{
			_context.Remove(roomType);
            return Save();
		}

		public RoomType GetRoomtype(int roomId)
        {
            return _context.RoomTypes.Where(s => s.TypeId == roomId).FirstOrDefault();
        }

        public ICollection<RoomType> GetRoomtypes()
        {
            return _context.RoomTypes.ToList();
        }

        public bool roomtypeexist(int roomId)
        {
            if (_context.RoomTypes.Any(t => t.TypeId == roomId))
                return true;
            else return false;
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

		public bool UpdateRoomType(RoomType roomType)
		{
			_context.Update(roomType);
            return Save();
		}
	}
}
