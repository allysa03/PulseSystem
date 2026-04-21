using InventorySystemDepEd.Api.Data;
using InventorySystemDepEd.Api.Models;
using InventorySystemDepEd.Shared.DTOs.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventorySystemDepEd.Api.Controllers;

[ApiController]
[Route("api/notifications")]
public class NotificationsController : ControllerBase
{
    private readonly AppDbContext _context;

    public NotificationsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/notifications/{userId}
    [HttpGet("{userId}")]
    public async Task<ActionResult<List<NotificationDto>>> GetByUser(int userId)
    {
        var list = await _context.Notifications
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new NotificationDto
            {
                NotificationId = x.NotificationId,
                UserId = x.UserId,
                Title = x.Title,
                Message = x.Message,
                Type = x.Type,
                Module = x.Module,
                ReferenceId = x.ReferenceId,
                IsRead = x.IsRead,
                PersonnelId = x.PersonnelId,
                Action = x.Action,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();

        return Ok(list);
    }

    // GET: api/notifications/unread/{userId}
    [HttpGet("unread/{userId}")]
    public async Task<ActionResult<int>> GetUnreadCount(int userId)
    {
        var count = await _context.Notifications
            .CountAsync(x => x.UserId == userId && !x.IsRead);

        return Ok(count);
    }

    // PUT: api/notifications/read/{id}
    [HttpPut("read/{id}")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var notif = await _context.Notifications.FindAsync(id);
        if (notif == null) return NotFound();

        notif.IsRead = true;
        await _context.SaveChangesAsync();

        return Ok();
    }

    // PUT: api/notifications/read-all/{userId}
    [HttpPut("read-all/{userId}")]
    public async Task<IActionResult> MarkAllAsRead(int userId)
    {
        var list = await _context.Notifications
            .Where(x => x.UserId == userId && !x.IsRead)
            .ToListAsync();

        foreach (var n in list)
            n.IsRead = true;

        await _context.SaveChangesAsync();

        return Ok();
    }

    // POST: api/notifications
    [HttpPost]
    public async Task<IActionResult> Create(CreateNotificationDto dto)
    {
        var entity = new NotificationsModel
        {
            UserId = dto.UserId,
            Title = dto.Title,
            Message = dto.Message,
            Type = dto.Type,
            Module = dto.Module,
            ReferenceId = dto.ReferenceId ?? 0,
            PersonnelId = dto.PersonnelId ?? 0,
            Action = dto.Action,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        _context.Notifications.Add(entity);
        await _context.SaveChangesAsync();

        return Ok();
    }
}