namespace InventorySystemDepEd.Shared.DTOs.Notifications;

public class UpdateNotificationActionDto
{
    public int NotificationId { get; set; }
    public string Action { get; set; } = string.Empty; // Approved / Rejected
}