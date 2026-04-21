namespace InventorySystemDepEd.Shared.DTOs.Notifications;

public class CreateNotificationDto
{
    public int UserId { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    public string Type { get; set; } = "GENERAL";
    public string Module { get; set; } = string.Empty;

    public int? ReferenceId { get; set; }
    public int? PersonnelId { get; set; }

    public string Action { get; set; } = "Pending";
}