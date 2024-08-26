using HotelApp1.Domain.Interface;
using HotelApp1.DTO;
using HotelApp1.Entities.Data;
using HotelApp1.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelApp1.Domain.Repository
{
	public class ReservationRepository : IReservationRepository
	{
		private readonly HotelAppContext _context;

		public ReservationRepository(HotelAppContext context)
		{
			_context = context;
		}

		public bool CreateReservation(Reservation reservationDto, int clientId, int roomnumber, int serviceId)
		{
			//int rez = reservationDto.ReservationServices.Any(t => t.ReservationServicesId =);
			/*var clientExists = _context.Clients.Any(c => c.ClientId == clientId);
			if (!clientExists)
			{
				throw new Exception("Client does not exist.");
			}*/
			
			double price = GetRoomPrice(roomnumber);

			var clientReservation = new Reservation()
			{
				ClientId = clientId,
				RoomNumber = roomnumber,
				ReservationStatus = "pending",
				CheckInDate = reservationDto.CheckInDate,
				CheckOutDate = reservationDto.CheckOutDate,
				TotalPrice = price * (reservationDto.CheckOutDate - reservationDto.CheckInDate).TotalDays,
			};

			_context.Add(clientReservation);
			_context.SaveChanges();

			var reservationservice = _context.ExtraServices.Where(a => a.ServicesId == serviceId).FirstOrDefault();
			if (reservationservice == null) { reservationservice = null; }

			var extraReservation = new ReservationService()
			{
				ServicesId = reservationservice.ServicesId,
				ReservationId = clientReservation.ReservationId,
				ClientId = clientId,

			};
			_context.Add(extraReservation);
			
			return Save();


		}

		public ICollection<Reservation> GetAllReservations()
		{
			return _context.Reservations.ToList();
		}

		public Reservation GetReservation(int id)
		{
			return _context.Reservations.FirstOrDefault(s => s.ReservationId == id);
		}

		public bool ReservationExists(int id)
		{
			return _context.Reservations.Any(p => p.ReservationId == id);
		}

		public bool Save()
		{
			var saved = _context.SaveChanges();
			return saved > 0;
		}

		public double GetRoomPrice(int roomNumber)
		{
			var room = _context.Rooms
							   .Include(r => r.Type)
							   .FirstOrDefault(r => r.RoomNumber == roomNumber);

			if (room != null)
			{
				return room.Type.Price;
			}
			else
			{
				throw new ArgumentException($"Room with number {roomNumber} not found.");
			}
		}

		public bool IsRoomAvailable(int roomNumber, DateTime checkIn, DateTime checkOut)
		{

			return !_context.Reservations.Any(t => t.RoomNumber == roomNumber &&
		(t.CheckOutDate > checkIn && t.CheckInDate < checkOut) );
		}

		public bool UpdateReservation(Reservation reservationDto,int clientId ,int reservationId,int roomId, int serviceId)
		{
			// Retrieve the existing reservation
			var existingReservation = GetReservation(reservationId);
			if (existingReservation == null)
			{
				throw new Exception("Reservation not found.");
			}

			// Update the reservation properties
			existingReservation.CheckInDate = reservationDto.CheckInDate;
			existingReservation.CheckOutDate = reservationDto.CheckOutDate;
			existingReservation.TotalPrice = GetRoomPrice(existingReservation.RoomNumber) *
											 (reservationDto.CheckOutDate - reservationDto.CheckInDate).TotalDays;
			existingReservation.ReservationStatus = reservationDto.ReservationStatus;

			// Update the reservation
			_context.Update(existingReservation);
			_context.SaveChanges();

			// Retrieve or create the service
			var reservationService = _context.ExtraServices.FirstOrDefault(a => a.ServicesId == serviceId);
			if (reservationService == null)
			{
				throw new Exception("Service not found.");
			}

			// Check if the reservation service already exists
			var existingReservationService = _context.ReservationServices.FirstOrDefault(rs => rs.ReservationId == reservationId && rs.ServicesId == serviceId);
			if (existingReservationService == null)
			{
				// Add new reservation service
				var newReservationService = new ReservationService()
				{
					ServicesId = reservationService.ServicesId,
					ReservationId = existingReservation.ReservationId,
					ClientId = existingReservation.ClientId,
				};
				_context.Add(newReservationService);
			}
			else
			{
				// Update existing reservation service
				existingReservationService.ServicesId = reservationService.ServicesId;
				_context.Update(existingReservationService);
			}

			return Save();
		}


		public ICollection<Reservation> GetReservationsByClientId(int clientId)
		{
			return _context.Reservations.Where(r => r.ClientId == clientId).ToList();
		}

		public bool DeleteReservation(Reservation reservationDto)
		{
			_context.Remove(reservationDto);
			return Save();
		}

		public bool DeleteReservations(List<Reservation> reservations)
		{
			_context.RemoveRange(reservations);
			return Save();
		}
	}
}
