namespace InventorySystemDepEd.Shared.DTOs.Notifications;

public class NotificationDto
{
    public int NotificationId { get; set; }
    public int UserId { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;
    public string Module { get; set; } = string.Empty;

    public int ReferenceId { get; set; } 

    public bool IsRead { get; set; }

    public int? PersonnelId { get; set; }

    public string Action { get; set; } = "Pending";

    public DateTime CreatedAt { get; set; }
}