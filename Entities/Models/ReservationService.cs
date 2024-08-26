using System;
using System.Collections.Generic;

namespace HotelApp1.Entities.Models
{
    public partial class ReservationService
    {
        public int ReservationServicesId { get; set; }
        public int ReservationId { get; set; }
        public int ServicesId { get; set; }
        public int ClientId { get; set; }

        public virtual Client Client { get; set; } = null!;
        public virtual Reservation Reservation { get; set; } = null!;
        public virtual ExtraService Services { get; set; } = null!;
    }
}
