namespace RaceDay.Web.Models
{
    public class CreateEventForm
    {
        public int VenueId { get; set; } = 1;
        public string EventName { get; set; } = "";
        public string EventType { get; set; } = "Running";
        public DateTime EventDate { get; set; } = DateTime.Today.AddDays(30);
        public string Description { get; set; } = "";
        public string RouteInfo { get; set; } = "";
    }
}