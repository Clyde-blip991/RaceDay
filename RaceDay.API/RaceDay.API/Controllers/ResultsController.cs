using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class ResultsController : ControllerBase
    {
        private readonly RaceDayContext _db;
        public ResultsController(RaceDayContext db) { _db = db; }

        public record ResultRequest(string FinishTime, int Position);

        // Organiser records a finish time and position for an enrolment
        [HttpPost("enrolments/{enrolmentId}/result")]
        public async Task<IActionResult> CaptureResult(
            int enrolmentId,
            [FromHeader(Name = "X-User-Id")] int userId,
            ResultRequest req)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user == null) return Unauthorized("Unknown user");
            if (user.Role != "Organiser") return StatusCode(403, "Only Organisers can capture results");

            var enrolment = await _db.Enrolments
                .Include(e => e.Result)
                .FirstOrDefaultAsync(e => e.EnrolmentId == enrolmentId);
            if (enrolment == null) return NotFound("Enrolment not found");
            if (enrolment.Result != null) return Conflict("Result already captured");

            if (!TimeSpan.TryParse(req.FinishTime, out var finish))
                return BadRequest("FinishTime must look like 00:45:30");

            var result = new Result
            {
                EnrolmentId = enrolmentId,
                FinishTime = finish,
                Position = req.Position,
                CapturedByUserId = userId,
                CapturedAt = DateTime.UtcNow
            };
            _db.Results.Add(result);
            await _db.SaveChangesAsync();
            return StatusCode(201, new { result.ResultId, result.Position });
        }

        // Participant views their own results
        [HttpGet("results/me")]
        public async Task<IActionResult> MyResults([FromHeader(Name = "X-User-Id")] int userId)
        {
            var list = await _db.Results
                .Include(r => r.Enrolment).ThenInclude(e => e.Category).ThenInclude(c => c.Event)
                .Where(r => r.Enrolment.ParticipantId == userId)
                .Select(r => new
                {
                    r.ResultId,
                    EventName = r.Enrolment.Category.Event.EventName,
                    EventDate = r.Enrolment.Category.Event.EventDate,
                    CategoryName = r.Enrolment.Category.CategoryName,
                    FinishTime = r.FinishTime.HasValue ? r.FinishTime.Value.ToString(@"hh\:mm\:ss") : null,
                    r.Position
                })
                .ToListAsync();
            return Ok(list);
        }
    }
}