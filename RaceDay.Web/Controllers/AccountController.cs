
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Models;
using System.Net.Http.Json;
using System.Security.Claims;

namespace RaceDay.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var client = _httpClientFactory.CreateClient("RaceDayApi");

                var response = await client.PostAsJsonAsync(
                    "api/auth/register",
                    new
                    {
                        model.FirstName,
                        model.LastName,
                        model.Email,
                        model.Password,
                        Role = "Participant"
                    });

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] =
                        "Registration successful. Please log in.";

                    return RedirectToAction(nameof(Login));
                }

                ModelState.AddModelError(
                    "",
                    "Registration failed. Please check your details.");
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(
                    "",
                    "Unable to connect to RaceDay API.");
            }

            return View(model);
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var client = _httpClientFactory.CreateClient("RaceDayApi");

                using var response = await client.PostAsJsonAsync(
                    "api/auth/login",
                    new
                    {
                        model.Email,
                        model.Password
                    });

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(
                        "",
                        "Invalid email or password.");

                    return View(model);
                }

                var user = await response.Content
                    .ReadFromJsonAsync<LoginResponse>();

                if (user == null ||
                    user.UserID <= 0 ||
                    string.IsNullOrWhiteSpace(user.Email) ||
                    (user.Role != "Participant" &&
                     user.Role != "Organiser"))
                {
                    ModelState.AddModelError(
                        "",
                        "Invalid response from the RaceDay API.");

                    return View(model);
                }

                // Capture the API session cookie.
                var apiSessionCookie = response.Headers
                    .TryGetValues("Set-Cookie", out var cookieHeaders)
                    ? cookieHeaders
                        .Select(header => header.Split(';')[0].Trim())
                        .FirstOrDefault(cookie =>
                            cookie.StartsWith(
                                ".AspNetCore.Session=",
                                StringComparison.Ordinal))
                    : null;

                if (string.IsNullOrWhiteSpace(apiSessionCookie))
                {
                    ModelState.AddModelError(
                        "",
                        "The RaceDay API did not return a session.");

                    return View(model);
                }

                // Store the API cookie in the MVC server-side session.
                HttpContext.Session.SetString(
                    "RaceDayApiSessionCookie",
                    apiSessionCookie);

                // Create MVC authentication claims.
                var claims = new List<Claim>
                {
                    new Claim(
                        ClaimTypes.NameIdentifier,
                        user.UserID.ToString()),

                    new Claim(
                        ClaimTypes.Name,
                        user.Email),

                    new Claim(
                        ClaimTypes.Role,
                        user.Role)
                };

                var identity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal);

                // Redirect users to their role dashboard.
                if (user.Role == "Organiser")
                    return RedirectToAction("Index", "Organiser");

                return RedirectToAction("Index", "Participant");
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(
                    "",
                    "Unable to connect to RaceDay API.");
            }
            catch (System.Text.Json.JsonException)
            {
                ModelState.AddModelError(
                    "",
                    "Invalid response from RaceDay API.");
            }

            return View(model);
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Remove the stored API session reference.
            HttpContext.Session.Remove("RaceDayApiSessionCookie");

            // Remove MVC authentication.
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(Login));
        }

        // GET: /Account/AccessDenied
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return Content("Access denied.");
        }

        private class LoginResponse
        {
            public int UserID { get; set; }
            public string Email { get; set; } = "";
            public string Role { get; set; } = "";
        }
    }
}
