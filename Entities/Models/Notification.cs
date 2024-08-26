using System;
using System.Collections.Generic;

namespace HotelApp1.Entities.Models
{
    public partial class Notification
    {
        public int NotificationId { get; set; }
        public int SenderClientId { get; set; }
        public int ReceiverClientId { get; set; }
        public bool Seen { get; set; }
        public int ReservationId { get; set; }

        public virtual Client ReceiverClient { get; set; } = null!;
        public virtual Reservation Reservation { get; set; } = null!;
        public virtual Client SenderClient { get; set; } = null!;
    }
}
