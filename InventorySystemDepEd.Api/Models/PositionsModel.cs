using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventorySystemDepEd.Api.Models
{
    [Table("tbl_positions")]
    public class PositionsModel
    {
        [Key]
        [Column("pos_id")]
        public int PositionId { get; set; }

        [Column("pos_name")]
        public string? PositionName { get; set; }

        // Navigation: One Position -> Many Personnels
        public ICollection<PersonnelsModel>? Personnels { get; set; }
    }
}