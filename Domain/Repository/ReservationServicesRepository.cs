using HotelApp1.Domain.Interface;
using HotelApp1.Entities.Data;
using HotelApp1.Entities.Models;

namespace HotelApp1.Domain.Repository
{
	public class ReservationServicesRepository : IReservationServicesReporsitory
    {
        private readonly HotelAppContext _context;
		private readonly IReservationRepository _reservationRepository;

		public ReservationServicesRepository(HotelAppContext context, IReservationRepository reservationRepository)
        {
            _context = context;
			_reservationRepository = reservationRepository;
		}
		public bool ReservationExists(int id)
		{
			return _context.Reservations.Any(p => p.ReservationId == id);
		}

		public bool CreateReservationService(ReservationService reservationService)
		{
            if(!ReservationExists(reservationService.ReservationId))
            {
                throw new ArgumentException("Rezervimi nuk ekziston");
            };
            
            var rezervation = new ReservationService()
            {
                ReservationId = reservationService.ReservationId,
                ClientId = reservationService.ClientId,
                ServicesId = reservationService.ServicesId,
			};
			_context.ReservationServices.Add(reservationService);
            return Save();
		}

		public ReservationService GetReservationService(int id)
        {
            return _context.ReservationServices.Where(t => t.ReservationServicesId == id).FirstOrDefault();
        }

        public ICollection<ReservationService> GetReservationServices()
        {
            return _context.ReservationServices.ToList();
        }

        public bool ResevationSeviceExists(int id)
        {
            if (_context.ReservationServices.Any(p => p.ReservationServicesId == id))
                return true;
            else return false;
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

		public bool UpdateReservationService(ReservationService reservationService)
		{
			_context.Update(reservationService);
            return Save();
		}

		public bool DeleteReservationService(ReservationService reservationService)
		{
			_context.Remove(reservationService);
            return Save();
		}

		public ICollection<ReservationService> GetAllReservationServicesbyReservation(int id)
		{
			return _context.ReservationServices.Where(t=> t.ReservationId == id).ToList();
		}

		public bool DeleteReservationServices(List<ReservationService> reservationService)
		{
			_context.RemoveRange(reservationService);
            return Save();
		}

		public ICollection<ReservationService> GetReservationServicesByClientId(int clientId)
		{
			return _context.ReservationServices.Where(f=> f.ClientId == clientId).ToList();
		}
	}
}
