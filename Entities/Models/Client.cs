using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HotelApp1.Entities.Models
{
    public class Client
    {
        public Client()
        {
            NotificationReceiverClients = new HashSet<Notification>();
            NotificationSenderClients = new HashSet<Notification>();
            ReservationServices = new HashSet<ReservationService>();
            Reservations = new HashSet<Reservation>();
        }

        public int ClientId { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateTime Birth { get; set; }
        public string PhoneNumber { get; set; } = null!;

        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;


        public string Role { get; set; } = "User"; //Default role

        public virtual ICollection<Notification> NotificationReceiverClients { get; set; }
        public virtual ICollection<Notification> NotificationSenderClients { get; set; }
        public virtual ICollection<ReservationService> ReservationServices { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
		public virtual ICollection<BrowsingData> BrowsingData { get; set; }


	}
}
