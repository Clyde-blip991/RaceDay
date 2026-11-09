namespace RaceDay.Web.Models
{
    public class EnrolmentInfo
    {
        public int EnrolmentId { get; set; }
        public string Status { get; set; } = "";
        public DateTime EnrolmentDate { get; set; }
        public string EventName { get; set; } = "";
        public string CategoryName { get; set; } = "";
    }
}