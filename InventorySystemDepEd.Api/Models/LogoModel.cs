using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventorySystemDepEd.Api.Models
{
    [Table("tbl_logos")]
    public class LogoModel
    {
        [Key]
        [Column("logo_id")]
        public int LogoId { get; set; }

        [Column("logo_type")]
        public string LogoType { get; set; } = "";

        [Column("logo_data")]
        public byte[] ImageData { get; set; } = Array.Empty<byte>();

        [Column("logo_contentType")]
        public string ContentType { get; set; } = "";
    }
}