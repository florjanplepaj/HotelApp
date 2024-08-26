using HotelApp1.DTO;
using HotelApp1.Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp1.Domain.Interface
{
    public interface IReservationRepository
    {
        ICollection<Reservation> GetAllReservations();
        Reservation GetReservation(int id);
        ICollection<Reservation> GetReservationsByClientId(int clientId);
		bool CreateReservation(Reservation reservationDto, int clientId, int roomnumber,int serviceId);
        bool UpdateReservation(Reservation reservationDto, int clientId, int reservationId, int roomnumber, int serviceId);
		bool DeleteReservation(Reservation reservationDto);
        bool DeleteReservations(List<Reservation> reservations);
        bool ReservationExists(int id);
        double GetRoomPrice(int roomNumber);
        
		bool IsRoomAvailable(int roomNumber, DateTime checkIn, DateTime checkOut);

		bool Save();

    }

}
