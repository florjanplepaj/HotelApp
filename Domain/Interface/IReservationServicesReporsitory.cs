using HotelApp1.Entities.Models;

namespace HotelApp1.Domain.Interface
{
    public interface IReservationServicesReporsitory
    {
        ICollection<ReservationService> GetReservationServices();
        ReservationService GetReservationService(int id);
        ICollection<ReservationService> GetReservationServicesByClientId(int clientId);
        ICollection <ReservationService> GetAllReservationServicesbyReservation(int id);
		bool CreateReservationService(ReservationService reservationService);
        bool DeleteReservationService(ReservationService reservationService);
        bool DeleteReservationServices(List<ReservationService> reservationService);
		bool UpdateReservationService(ReservationService reservationService);
		bool ResevationSeviceExists(int id);
        bool Save();
    }
}
