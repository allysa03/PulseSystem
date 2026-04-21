using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventorySystemDepEd.Api.Models
{
    [Table("tbl_users")]
    public class UsersModel
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [EmailAddress]
        [Column("user_emailAddress")]
        public string UserEmailAddress { get; set; } = null!;

        [Required]
        [Column("user_password")]
        public string UserPassword { get; set; } = null!;

        [Column("user_role")]
        public int? UserRole { get; set; }

        [Column("user_IsApproved")]
        public bool UserIsApproved { get; set; }

        [Column("user_isLocked")]
        public bool UserIsLocked { get; set; }

        [Column("user_IsDeleted")]
        public bool UserIsDeleted { get; set; }

        [Column("user_logAttempt")]
        public int UserLogAttempt { get; set; }

        [Column("user_lastLog")]
        public DateTime? UserLastLog { get; set; }

        [Column("user_createdAt")]
        public DateTime? UserCreatedAt { get; set; } = DateTime.UtcNow;

        [Column("user_approvedAt")]
        public DateTime? UserApprovedAt { get; set; }

        [Column("user_modifiedAt")]
        public DateTime? UserModifiedAt { get; set; }       

        [ForeignKey("UserRole")]
        public RolesModel? Role { get; set; }

        [Column("user_status")]
        [MaxLength(50)]
        public string UserStatus { get; set; } = "Pending";

        [Column("user_termAndCon")]
        public bool TermAndCon { get; set; } = false;

        // Explicit 1:1 mapping to Personnel (navigation without [ForeignKey] here)
        public PersonnelsModel? Personnel { get; set; }

        // Helper
        [NotMapped]
        public string? FullName => Personnel != null
            ? $"{Personnel.FirstName} {Personnel.MiddleName} {Personnel.LastName}".Replace("  ", " ").Trim()
            : null;
    }
}