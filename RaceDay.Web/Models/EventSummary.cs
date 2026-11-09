namespace RaceDay.Web.Models
{
    public class EventSummary
    {
        public int EventId { get; set; }
        public string EventName { get; set; } = "";
        public string EventType { get; set; } = "";
        public DateTime EventDate { get; set; }
    }
}