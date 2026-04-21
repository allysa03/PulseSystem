using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventorySystemDepEd.Api.Models
{
    [Table("tbl_offices")]
    public class OfficesModel
    {
        [Key]
        [Column("office_id")]
        public int OfficeId { get; set; }

        [Column("office_name")]
        public string? OfficeName { get; set; }

        [Column("office_contactNumber")]
        public string? ContactNumber { get; set; }

        [Column("office_location")]
        public string? Location { get; set; }

        [Column("office_head")]
        public int? Head { get; set; }

        [Column("office_deparment")]
        public int? DepartmentId { get; set; }

        // ✅ Office Head navigation
        [ForeignKey("Head")]
        public PersonnelsModel? HeadPersonnel { get; set; }

        // Department navigation
        [ForeignKey("DepartmentId")]
        public DepartmentsModel? Department { get; set; }

        public ICollection<PersonnelsModel>? Personnels { get; set; }
    }
}