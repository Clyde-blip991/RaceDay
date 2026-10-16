using Microsoft.Extensions.Logging;

namespace RaceDay.API.Models
{
    public class Venue
    {
        public int VenueId { get; set; }
        public string VenueName { get; set; } = null!;
        public string AddressLine { get; set; } 
        public string City { get; set; } = null!;
        public string Province { get; set; } 
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}