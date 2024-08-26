using HotelApp1.Entities.Models;

namespace HotelApp1.Domain.Interface
{
    public interface IRoomRepository
    {
        ICollection<Room> GetRooms();
        Room GetRoom(int roomId);
        bool CreateRoom(Room room);
        bool UpdateRoom(Room room);
        bool DeleteRoom(Room room);
        bool roomExist(int roomId);
        bool Save();
    }
}
