//using InventorySystemDepEd.Data;
//using InventorySystemDepEd.Models;
using Microsoft.EntityFrameworkCore;

namespace InventorySystemDepEd.Services
{
    public class NotificationServiceEfCore
    {
        //private readonly AppDbContext _context;

        //public NotificationServiceEfCore(AppDbContext context)
        //{
        //    _context = context;
        //}

        //public async Task<List<NotificationsModel>> GetNotificationsByUserIdAsync(int userId)
        //{
        //    return await _context.Notifications
        //                         .Where(n => n.UserId == userId)
        //                         .OrderByDescending(n => n.CreatedAt)
        //                         .ToListAsync();
        //}

        //public async Task MarkAsReadAsync(int notificationId)
        //{
        //    var notification = await _context.Notifications.FindAsync(notificationId);
        //    if (notification != null && !notification.IsRead)
        //    {
        //        notification.IsRead = true;
        //        await _context.SaveChangesAsync();
        //    }
        //}

        //public async Task MarkNotificationsActionAsync(int transferId, int userId, string action)
        //{
        //    var notifications = await _context.Notifications
        //        .Where(n => n.ReferenceId == transferId && n.UserId == userId)
        //        .ToListAsync();

        //    foreach (var n in notifications)
        //        n.Action = action;

        //    await _context.SaveChangesAsync();
        //}

        //public async Task MarkTransferNotificationsAsReadAsync(int transferId, int userId)
        //{
        //    var notifications = await _context.Notifications
        //        .Where(n => n.ReferenceId == transferId && n.UserId == userId && !n.IsRead)
        //        .ToListAsync();

        //    foreach (var n in notifications)
        //        n.IsRead = true;

        //    await _context.SaveChangesAsync();
        //}

        //public async Task AddNotificationAsync(NotificationsModel notification)
        //{
        //    await _context.Notifications.AddAsync(notification);
        //    await _context.SaveChangesAsync();
        //}

        //public async Task NotifyUserAsync(int userId, string title, string message, string type = "info", string module = "", int? referenceId = null, int? forTransferID = null)
        //{
        //    var notification = new NotificationsModel
        //    {
        //        UserId = userId,
        //        Title = title,
        //        Message = message,
        //        Type = type,
        //        Module = module,
        //        ReferenceId = referenceId,
        //        PersonnelId = forTransferID,
        //        IsRead = false,
        //        CreatedAt = DateTime.UtcNow
        //    };

        //    await AddNotificationAsync(notification);
        //}

        //// ✅ Add this wrapper so CreateNotificationAsync works in OfficeServiceEfCore
        //public async Task CreateNotificationAsync(
        //    int recipientId,
        //    string title,
        //    string message,
        //    string category,
        //    int? referenceId = null,
        //    string? type = null)
        //{
        //    await NotifyUserAsync(
        //        recipientId,
        //        title,
        //        message,
        //        type ?? "info",
        //        category,
        //        referenceId ?? 0
        //    );
        //}
    }
}