using InventorySystemDepEd.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventorySystemDepEd.Shared.DTOs.Profile;
using InventorySystemDepEd.Shared.DTOs.Personnels;
using InventorySystemDepEd.Shared.DTOs;

namespace InventorySystemDepEd.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProfileController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetProfile(int userId)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Personnel!)
                    .ThenInclude(p => p.Office!)
                        .ThenInclude(o => o.Department)
                .Include(u => u.Personnel!)
                    .ThenInclude(p => p.Position)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
                return NotFound();

            var personnel = user.Personnel;

            var dto = new UserProfileDto
            {
                UserId = user.UserId,
                UserEmailAddress = user.UserEmailAddress ?? "",
                UserIsApproved = user.UserIsApproved,
                RoleName = user.Role?.RoleName ?? "",

                Personnel = new PersonnelDto
                {
                    PersonnelId = personnel?.PersonnelId ?? 0,
                    EmployeeID = personnel?.EmployeeID ?? "",
                    FirstName = personnel?.FirstName ?? "",
                    MiddleName = personnel?.MiddleName ?? "",
                    LastName = personnel?.LastName ?? "",
                    ContactNumber = personnel?.ContactNumber ?? "",
                    HiredDate = personnel?.HiredDate,
                    PositionName = personnel?.Position?.PositionName ?? "",

                    Office = personnel?.Office == null
                    ? null
                    : new OfficeDto
                    {
                        OfficeName = personnel.Office.OfficeName ?? "",
                        DepartmentName = personnel.Office.Department?.DepartmentName ?? ""
                    }
                }
            };

            return Ok(dto);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var user = await _context.Users.FindAsync(dto.UserId);

            if (user == null)
                return NotFound("User not found");

            if (string.IsNullOrEmpty(user.UserPassword) ||
                user.UserPassword != dto.CurrentPassword)
                return BadRequest("Current password is incorrect");

            user.UserPassword = dto.NewPassword;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Password updated successfully" });
        }
    }
}