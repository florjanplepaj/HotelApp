using System;
using System.Collections.Generic;

namespace HotelApp1.Entities.Models
{
    public partial class ExtraService
    {
        public ExtraService()
        {
            ReservationServices = new HashSet<ReservationService>();
        }

        public int ServicesId { get; set; }
        public string Type { get; set; } = null!;
        public double Price { get; set; }
        public string Description { get; set; } = null!;

        public virtual ICollection<ReservationService> ReservationServices { get; set; }
    }
}
