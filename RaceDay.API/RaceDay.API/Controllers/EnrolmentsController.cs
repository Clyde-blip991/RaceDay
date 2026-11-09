using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class EnrolmentsController : ControllerBase
    {
        private readonly RaceDayContext _db;
        public EnrolmentsController(RaceDayContext db) { _db = db; }

        [HttpPost("categories/{categoryId}/enrol")]
        public async Task<IActionResult> Enrol(int categoryId, [FromHeader(Name = "X-User-Id")] int userId)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user == null) return Unauthorized("Unknown user");
            if (user.Role != "Participant") return StatusCode(403, "Only Participants can enrol");

            var category = await _db.Categories.FindAsync(categoryId);
            if (category == null) return NotFound("Category not found");

            var alreadyEnrolled = await _db.Enrolments
                .AnyAsync(e => e.ParticipantId == userId && e.CategoryId == categoryId);
            if (alreadyEnrolled) return Conflict("Already enrolled in this category");

            var enrolment = new Enrolment
            {
                ParticipantId = userId,
                CategoryId = categoryId,
                EnrolmentDate = DateTime.UtcNow,
                Status = "Confirmed"
            };
            _db.Enrolments.Add(enrolment);
            await _db.SaveChangesAsync();
            return StatusCode(201, new { enrolment.EnrolmentId, enrolment.Status });
        }

        [HttpGet("enrolments/me")]
        public async Task<IActionResult> MyEnrolments([FromHeader(Name = "X-User-Id")] int userId)
        {
            var list = await _db.Enrolments
                .Include(e => e.Category).ThenInclude(c => c.Event)
                .Where(e => e.ParticipantId == userId)
                .Select(e => new
                {
                    e.EnrolmentId,
                    e.Status,
                    e.EnrolmentDate,
                    EventName = e.Category.Event.EventName,
                    CategoryName = e.Category.CategoryName
                })
                .ToListAsync();
            return Ok(list);
        }
    }
}
