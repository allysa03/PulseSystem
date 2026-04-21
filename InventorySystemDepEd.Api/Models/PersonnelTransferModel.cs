using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventorySystemDepEd.Api.Models
{
    [Table("tbl_transfer")]
    public class PersonnelTransferModel
    {
        [Key]
        [Column("transfer_id")]
        public int TransferId { get; set; }

        [Column("transfer_personnel")]
        public int PersonnelId { get; set; }

        [Column("transfer_origin")]
        public int OriginOfficeId { get; set; }

        [Column("transfer_destination")]
        public int DestinationOfficeId { get; set; }

        [Column("transfer_remark")]
        public string? Remark { get; set; }

        [Column("transfer_rejectRemark")]
        public string? RejectRemark { get; set; }

        [Column("transfer_status")]
        public string Status { get; set; } = "Pending";

        [Column("transfer_date")]
        public DateTime? TransferDate { get; set; }

        [Column("transfer_approvedDate")]
        public DateTime? ApprovedDate { get; set; }

        [Column("transfer_createdBy")]
        public int CreatedBy { get; set; }

        [Column("transfer_approvedBy")]
        public int? ApprovedBy { get; set; }
    }
}