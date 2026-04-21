using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventorySystemDepEd.Api.Models
{
    [Table("tbl_notifications")]
    public class NotificationsModel
    {
        [Key]
        [Column("notification_id")]
        public int NotificationId { get; set; }

        [Column("notification_userID")]
        public int UserId { get; set; }

        [Column("notification_title")]
        public string Title { get; set; } = string.Empty;

        [Column("notification_message")]
        public string Message { get; set; } = string.Empty;

        [Column("notification_type")]
        public string Type { get; set; } = string.Empty;

        [Column("notification_module")]
        public string Module { get; set; } = string.Empty;

        [Column("notification_refID")]
        public int ReferenceId { get; set; }

        [Column("notification_isRead")]
        public bool IsRead { get; set; } = false;

        [Column("notification_approvedTransferID")]
        public int? PersonnelId { get; set; }

        [Column("notification_action")]
        public string Action { get; set; } = "Pending";

        [Column("notification_createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}