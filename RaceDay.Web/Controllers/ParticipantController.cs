using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Models;
using RaceDay.Web.Services;

namespace RaceDay.Web.Controllers
{
    public class ParticipantController : Controller
    {
        private readonly ApiClient _api;
        private static readonly JsonSerializerOptions Json = new() { PropertyNameCaseInsensitive = true };

        public ParticipantController(ApiClient api) { _api = api; }

        private bool IsParticipant() =>
            HttpContext.Session.GetInt32("UserId") != null &&
            HttpContext.Session.GetString("Role") == "Participant";

        public async Task<IActionResult> Details(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
                return RedirectToAction("Login", "Account");

            var (ok, _, body) = await _api.GetAsync($"api/events/{id}");
            if (!ok) return NotFound();

            var ev = JsonSerializer.Deserialize<EventDetails>(body, Json)!;
            return View(ev);
        }

        [HttpPost]
        public async Task<IActionResult> Enrol(int categoryId, int eventId)
        {
            if (!IsParticipant()) return RedirectToAction("Login", "Account");

            var userId = HttpContext.Session.GetInt32("UserId")!.Value;
            var (ok, status, body) = await _api.PostAsync(
                $"api/categories/{categoryId}/enrol", new { }, userId);

            TempData["Message"] = ok
                ? "You are enrolled."
                : $"Could not enrol ({status}): {body}";
            return RedirectToAction("Details", new { id = eventId });
        }

        public async Task<IActionResult> MyEnrolments()
        {
            if (!IsParticipant()) return RedirectToAction("Login", "Account");

            var userId = HttpContext.Session.GetInt32("UserId")!.Value;
            var (ok, _, body) = await _api.GetAsync("api/enrolments/me", userId);
            if (!ok) return View(new List<EnrolmentInfo>());

            var list = JsonSerializer.Deserialize<List<EnrolmentInfo>>(body, Json) ?? new();
            return View(list);
        }
    }
}