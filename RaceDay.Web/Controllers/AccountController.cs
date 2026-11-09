using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Services;

namespace RaceDay.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApiClient _api;
        public AccountController(ApiClient api) { _api = api; }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var (ok, status, body) = await _api.PostAsync("api/auth/login", new { email, password });
            if (!ok)
            {
                ViewBag.Error = "Invalid email or password.";
                return View();
            }

            var data = System.Text.Json.JsonDocument.Parse(body).RootElement;
            HttpContext.Session.SetInt32("UserId", data.GetProperty("userId").GetInt32());
            HttpContext.Session.SetString("Role", data.GetProperty("role").GetString()!);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(string fullName, string email, string password, string role, string phoneNumber)
        {
            var (ok, status, body) = await _api.PostAsync("api/auth/register",
                new { fullName, email, password, role, phoneNumber });
            if (!ok)
            {
                ViewBag.Error = body;
                return View();
            }
            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}