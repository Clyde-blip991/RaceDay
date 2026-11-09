using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Models;
using RaceDay.Web.Services;

namespace RaceDay.Web.Controllers
{
    public class OrganiserController : Controller
    {
        private readonly ApiClient _api;
        public OrganiserController(ApiClient api) { _api = api; }

        private bool IsOrganiser() =>
            HttpContext.Session.GetInt32("UserId") != null &&
            HttpContext.Session.GetString("Role") == "Organiser";

        public async Task<IActionResult> Events()
        {
            if (!IsOrganiser()) return RedirectToAction("Login", "Account");

            var (ok, _, body) = await _api.GetAsync("api/events");
            var events = ok
                ? JsonSerializer.Deserialize<List<EventSummary>>(body,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new()
                : new List<EventSummary>();
            return View(events);
        }

        public IActionResult Create()
        {
            if (!IsOrganiser()) return RedirectToAction("Login", "Account");
            return View(new CreateEventForm());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEventForm form)
        {
            if (!IsOrganiser()) return RedirectToAction("Login", "Account");

            var userId = HttpContext.Session.GetInt32("UserId")!.Value;
            var (ok, status, body) = await _api.PostAsync("api/events", new
            {
                form.VenueId,
                form.EventName,
                form.EventType,
                form.EventDate,
                form.Description,
                form.RouteInfo
            }, userId);

            if (!ok)
            {
                ViewBag.Error = $"Could not create event ({status}).";
                return View(form);
            }
            TempData["Message"] = "Event created.";
            return RedirectToAction("Events");
        }
    }
}
