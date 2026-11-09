namespace RaceDay.Web.Models
{
    public class ResultInfo
    {
        public int ResultId { get; set; }
        public string EventName { get; set; } = "";
        public DateTime EventDate { get; set; }
        public string CategoryName { get; set; } = "";
        public string? FinishTime { get; set; }
        public int? Position { get; set; }
    }
}