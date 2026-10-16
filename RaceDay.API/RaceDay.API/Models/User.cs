using Microsoft.Extensions.Logging;

namespace RaceDay.API.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public ICollection<Event> OrganisingEvents { get; set; } = new List<Event>();
        public ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();
        public ICollection<Result> CapturedResults { get; set; } = new List<Result>();
    }
}