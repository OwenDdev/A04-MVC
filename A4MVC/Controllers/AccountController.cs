using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Claims;

namespace Assignment4MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            IActionResult result = View();
            return result;
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            IActionResult result;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Username and password are required.");
                result = View();
                return result;
            }

            // Validate credentials against the database
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            string foundUsername = "";
            string residentId = "";
            bool isValid = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlQuery = "SELECT residentid, username FROM Residentuser WHERE username = @Username AND userpassword = @Password";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isValid = true;
                            foundUsername = reader["username"].ToString();
                            residentId = reader["residentid"].ToString();
                        }
                    }
                }
            }

            if (!isValid)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                result = View();
                return result;
            }

            // Credential is valid, retrieve catalogId for the resident
            string catalogId = "";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlQuery = "SELECT catalogid FROM Resident WHERE residentid = @ResidentId";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@ResidentId", residentId);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            catalogId = reader["catalogid"].ToString();
                        }
                    }
                }
            }

            // Create claims and sign in the user
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, foundUsername),
                new Claim("ResidentId", residentId),
                new Claim("CatalogId", catalogId)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties authProperties = new AuthenticationProperties
            {
                IsPersistent = false,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            result = LocalRedirect(returnUrl);
            return result;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            IActionResult result = RedirectToAction("Index", "Home");
            return result;
        }
    }
}