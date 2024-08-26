using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HotelApp1.Entities.Models
{
    public partial class BrowsingData
    {
        public int BrowsingId { get; set; }
        public string ActionType { get; set; } = null!;
        public DateTime Time { get; set; }

		// New foreign key property
		[JsonIgnore]
		public int ClientId { get; set; }

		// New navigation property
		public virtual Client Client { get; set; } = null!;
	}
}
