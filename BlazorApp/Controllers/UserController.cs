using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using BlazorApp.Data;
using BlazorApp.Models;
using System.Security.Cryptography;
using System.Text;

namespace BlazorApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/users/signup
        [HttpPost("signup")]
        public async Task<ActionResult<User>> SignUp([FromBody] RegisterUser registerUser)
        {
            // Simulate loading delay
            await Task.Delay(3000);

            if (_context.Users.Any(u => u.Username == registerUser.Username))
            {
                return BadRequest("Username already exists");
            }

            var hashedPassword = HashPassword(registerUser.Password);

            var user = new User
            {
                Username = registerUser.Username,
                Email = registerUser.Email,
                Password = hashedPassword
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // POST: api/users/login
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login([FromBody] User loginUser)
        {
            // Simulate loading delay
            await Task.Delay(3000);

            var user = await _context.Users
                .Where(u => u.Username == loginUser.Username)
                .FirstOrDefaultAsync();

            if (user == null || !VerifyPassword(loginUser.Password, user.Password))
            {
                return Unauthorized("Invalid username or password");
            }

            // Authentication successful
            return Ok(user);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            var enteredHash = HashPassword(enteredPassword);
            return enteredHash == storedHash;
        }
    }
}