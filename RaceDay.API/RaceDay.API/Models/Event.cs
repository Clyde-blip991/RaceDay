namespace RaceDay.API.Models
{
    public class Event
    {
        public int EventId { get; set; }
        public int OrganiserId { get; set; }
        public int VenueId { get; set; }
        public string EventName { get; set; } = null!;
        public string EventType { get; set; } = null!;
        public DateTime EventDate { get; set; }
        public string Description { get; set; } 
        public string RouteInfo { get; set; } 
        public DateTime CreatedAt { get; set; }
        public User Organiser { get; set; } = null!;
        public Venue Venue { get; set; } = null!;
        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}