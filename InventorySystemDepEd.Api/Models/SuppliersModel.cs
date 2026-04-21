using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventorySystemDepEd.Api.Models
{
    [Table("tbl_suppliers")]
    public class SuppliersModel
    {
        [Key]
        [Column("supplier_id")]
        public int SupplierId { get; set; }

        [Required]
        [Column("supplier_name")]
        public string SupplierName { get; set; } = null!;

        [Column("supplier_address")]
        public string? SupplierAddress { get; set; }

        [Column("supplier_contactNumber")]
        public string? SupplierContactNumber { get; set; }

        [Column("supplier_emailAddress")]
        public string? SupplierEmailAddress { get; set; }

        [Column("supplier_tinNumber")]
        public string? SupplierTinNumber { get; set; }
    }
}