using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventorySystemDepEd.Api.Models
{
    [Table("tbl_personnels")]
    public class PersonnelsModel
    {
        [Key]
        [Column("personnel_id")]
        public int PersonnelId { get; set; }

        [Required]
        [Column("employeeID")]
        public string EmployeeID { get; set; } = null!;

        [Column("firstName")]
        public string? FirstName { get; set; }

        [Column("middleName")]
        public string? MiddleName { get; set; }

        [Column("lastName")]
        public string? LastName { get; set; }

        [Column("contactNumber")]
        public string? ContactNumber { get; set; }

        [Column("emailAddress")]
        public string? EmailAddress { get; set; }

        [Column("hiredDate")]
        public DateTime? HiredDate { get; set; }

        [Column("office")]
        public int? OfficeId { get; set; }

        [Column("accountID")]
        public int? AccountID { get; set; }

        [Column("position")]
        public int? PositionId { get; set; }

        [ForeignKey("AccountID")]
        public UsersModel? User { get; set; }

        [ForeignKey("OfficeId")]
        public OfficesModel? Office { get; set; }

        [ForeignKey("PositionId")]
        public PositionsModel? Position { get; set; }

        // ✅ ADD THIS
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }
}