using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventorySystemDepEd.Api.Models
{
    [Table("tbl_roles")]
    public class RolesModel
    {
        [Key]
        [Column("role_id")]
        public int RoleId { get; set; }

        [Required]
        [Column("role_name")]
        public string RoleName { get; set; } = null!;

        // Navigation: 1 role → many users
        public ICollection<UsersModel> Users { get; set; } = new List<UsersModel>();
    }
}