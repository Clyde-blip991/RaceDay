namespace RaceDay.Web.Models
{
    public class EventDetails
    {
        public int EventId { get; set; }
        public string EventName { get; set; } = "";
        public string EventType { get; set; } = "";
        public DateTime EventDate { get; set; }
        public string Description { get; set; } = "";
        public string RouteInfo { get; set; } = "";
        public List<CategoryInfo> Categories { get; set; } = new();
    }

    public class CategoryInfo
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = "";
        public decimal DistanceKm { get; set; }
        public decimal EntryFee { get; set; }
    }
}