using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using src.Data;
using src.Models;
using static src.Data.BasicStructureDbContext;

namespace src.Controllers
{
    public class UserController : Controller
    {
        private readonly ProjectDbContext _context;
        public UserController(ProjectDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUsername(string newUsername)
        {
            User? currentUser = _context.Users.Where(u => u.Id == HttpContext.Session.GetInt32("UserId")).FirstOrDefault();
            if (currentUser == null)
            {
                return RedirectToAction("Login");
            }
            if (!_context.Users.Where(u => u.Username == newUsername && u.Id != currentUser.Id).IsNullOrEmpty())
            {
                ViewData["UsernameError"] = "Uživatel s tímto jménem již existuje";
                return View("Profile", currentUser);
            }
            currentUser.Username = newUsername;
            await _context.SaveChangesAsync();
            return View("Profile", currentUser);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword)
        {
            User? currentUser = _context.Users.Where(u => u.Id == HttpContext.Session.GetInt32("UserId")).FirstOrDefault();
            if (currentUser == null)
            {
                return RedirectToAction("Login");
            }
            if (!BCrypt.Net.BCrypt.Verify(currentPassword, currentUser.Password))
            {
                ViewData["PasswordError"] = "Nesprávné heslo";
                return View("Profile", currentUser);
            }
            currentUser.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _context.SaveChangesAsync();
            return View("Profile", currentUser);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccountAsync()
        {
            User? currentUser = _context.Users.Where(u => u.Id == HttpContext.Session.GetInt32("UserId")).FirstOrDefault();

            if (currentUser == null)
            {
                return RedirectToAction("Login");
            }

            _context.Users.Remove(currentUser);
            await _context.SaveChangesAsync();

            // Odhlášení po smazání účtu
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult profile()
        {
            User? currentUser = _context.Users.Where(u => u.Id == HttpContext.Session.GetInt32("UserId")).FirstOrDefault();

            return View(currentUser);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (_context.Users.FirstOrDefault(u => u.Username == model.Username) != null)
            {
                // Přidání specifické chyby k poli Username
                ModelState.AddModelError("Username", "Uživatel s tímto jménem již existuje");
                return View(model);
            }

            var user = new User
            {
                Username = model.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Neplatné přihlašovací údaje");
            return View();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}