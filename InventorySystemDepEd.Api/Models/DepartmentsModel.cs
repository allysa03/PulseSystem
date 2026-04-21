using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventorySystemDepEd.Api.Models
{
    [Table("tbl_departments")]
    public class DepartmentsModel
    {
        [Key]
        [Column("department_id")]
        public int DepartmentId { get; set; }

        [Column("department_name")]
        public string DepartmentName { get; set; } = null!;

        [Column("department_contactNumber")]
        public string? ContactNumber { get; set; }

        [Column("department_location")]
        public string? Location { get; set; }

        [Column("department_head")]
        public int? Head { get; set; }

        // ✅ FIX: Navigation to Personnel (Department Head)
        [ForeignKey("Head")]
        public PersonnelsModel? HeadPersonnel { get; set; }

        public ICollection<OfficesModel>? Offices { get; set; }
    }
}