using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Models;
using RaceDay.Web.Services;

namespace RaceDay.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiClient _api;
        public HomeController(ApiClient api) { _api = api; }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
                return RedirectToAction("Login", "Account");

            var (ok, _, body) = await _api.GetAsync("api/events");
            var events = ok
                ? JsonSerializer.Deserialize<List<EventSummary>>(body,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new()
                : new List<EventSummary>();

            ViewBag.Role = HttpContext.Session.GetString("Role");
            return View(events);
        }

        public IActionResult Privacy() => View();

        public IActionResult Error() => View();
    }
}