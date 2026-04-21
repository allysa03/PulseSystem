using InventorySystemDepEd.Api.Data;
using InventorySystemDepEd.Api.Models;
using InventorySystemDepEd.Shared.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventorySystemDepEd.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new LoginResponse
                {
                    Success = false,
                    Message = "Email and password are required"
                });
            }

            var email = request.Email.Trim().ToLower();
            var password = request.Password.Trim();

            var user = await _context.Users
                .Include(u => u.Personnel!)
                    .ThenInclude(p => p.Office!)
                .Include(u => u.Personnel!)
                    .ThenInclude(p => p.Position)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u =>
                    !string.IsNullOrEmpty(u.UserEmailAddress) &&
                    u.UserEmailAddress.ToLower() == email);

            if (user == null)
            {
                return NotFound(new LoginResponse
                {
                    Success = false,
                    Message = "Unauthorized User or User not found"
                });
            }

            if (user.UserIsLocked)
            {
                return Unauthorized(new LoginResponse
                {
                    Success = false,
                    Message = "Your account is locked due to multiple failed login attempts. Please contact administrator."
                });
            }

            if (!user.UserIsApproved)
            {
                return Unauthorized(new LoginResponse
                {
                    Success = false,
                    Message = "Your account is not approved yet."
                });
            }

            if (string.IsNullOrEmpty(user.UserPassword) || user.UserPassword != password)
            {
                user.UserLogAttempt++;

                var attemptsLeft = 3 - user.UserLogAttempt;

                if (user.UserLogAttempt >= 3)
                {
                    user.UserIsLocked = true;
                    await _context.SaveChangesAsync();

                    return Unauthorized(new LoginResponse
                    {
                        Success = false,
                        Message = "Account locked due to 3 failed login attempts."
                    });
                }

                await _context.SaveChangesAsync();

                return Unauthorized(new LoginResponse
                {
                    Success = false,
                    Message = $"Invalid password. {attemptsLeft} attempt(s) remaining."
                });
            }

            user.UserLogAttempt = 0;
            user.UserLastLog = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var personnel = user.Personnel;

            return Ok(new LoginResponse
            {
                Success = true,
                Message = "Login successful",

                UserId = user.UserId,
                Email = user.UserEmailAddress ?? "",
                Role = user.Role?.RoleName ?? "User",

                PersonnelId = personnel?.PersonnelId ?? 0,
                OfficeId = personnel?.OfficeId ?? 0,

                FullName = personnel != null
                    ? $"{personnel.FirstName ?? ""} {personnel.LastName ?? ""}".Trim()
                    : (user.UserEmailAddress ?? "")
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new LoginResponse
                {
                    Success = false,
                    Message = "Email and password are required"
                });
            }

            var email = request.Email.Trim().ToLower();

            // =========================
            // 1. CHECK PERSONNEL EXISTS
            // =========================
            var personnel = await _context.Personnels
                .FirstOrDefaultAsync(p => p.EmailAddress != null && p.EmailAddress.ToLower() == email);
            // ⚠️ adjust field name if needed

            if (personnel == null)
            {
                return BadRequest(new LoginResponse
                {
                    Success = false,
                    Message = "Email not found in Personnel records"
                });
            }

            // =========================
            // 2. CHECK USER EXISTS
            // =========================
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.UserEmailAddress != null && u.UserEmailAddress.ToLower() == email);

            if (existingUser != null)
            {
                return BadRequest(new LoginResponse
                {
                    Success = false,
                    Message = "This personnel already has an account"
                });
            }

            // =========================
            // 3. CREATE USER
            // =========================
            var user = new UsersModel
            {
                UserEmailAddress = email,
                UserPassword = request.Password,
                UserIsApproved = false,
                UserIsLocked = false,
                TermAndCon = request.TermsAndConditions,
                UserLogAttempt = 0,
                UserRole = 5
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(); // IMPORTANT (generate UserId)

            // =========================
            // 4. LINK PERSONNEL → USER
            // =========================
            personnel.AccountID = user.UserId;

            await _context.SaveChangesAsync();

            return Ok(new LoginResponse
            {
                Success = true,
                Message = "Registration successful. Wait for admin approval."
            });
        }
    }
}