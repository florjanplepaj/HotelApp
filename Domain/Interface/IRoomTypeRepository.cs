using HotelApp1.Entities.Models;

namespace HotelApp1.Domain.Interface
{
    public interface IRoomTypeRepository
    {
        ICollection<RoomType> GetRoomtypes();
        RoomType GetRoomtype(int roomId);
        bool roomtypeexist(int roomId);
		bool CreateRoomType(RoomType roomType);
        bool DeleteRoomType(RoomType roomType);
		bool UpdateRoomType(RoomType roomType);
		bool Save();
    }
}
