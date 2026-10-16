using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        private readonly RaceDayContext _db;
        public EventsController(RaceDayContext db) { _db = db; }

        public record EventRequest(int VenueId, string EventName, string EventType, DateTime EventDate, string Description, string RouteInfo);

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _db.Events.Include(e => e.Categories).ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var ev = await _db.Events.Include(e => e.Categories).FirstOrDefaultAsync(e => e.EventId == id);
            return ev == null ? NotFound() : Ok(ev);
        }

        // Requires header X-User-Id with an Organiser's UserId
        [HttpPost]
        public async Task<IActionResult> Create([FromHeader(Name = "X-User-Id")] int userId, EventRequest req)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user == null) return Unauthorized("Unknown user");
            if (user.Role != "Organiser") return StatusCode(403, "Only Organisers can create events");

            var ev = new Event
            {
                OrganiserId = user.UserId,
                VenueId = req.VenueId,
                EventName = req.EventName,
                EventType = req.EventType,
                EventDate = req.EventDate,
                Description = req.Description,
                RouteInfo = req.RouteInfo,
                CreatedAt = DateTime.UtcNow
            };
            _db.Events.Add(ev);
            await _db.SaveChangesAsync();
            return StatusCode(201, new { ev.EventId, ev.EventName });
        }
    }
}