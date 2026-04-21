using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventorySystemDepEd.Api.Models
{
    [Table("tbl_settings")]
    public class SettingsModel
    {
        [Key]
        [Column("settings_id")]
        public int SettingId { get; set; }

        [Column("settings_documentType")]
        public string? SettingDocumentType { get; set; }

        [Column("settings_format")]
        public string? SettingFormat { get; set; }

        [Column("settings_numberSequence")]
        public int SettingNumberSequence { get; set; }

        [Column("settings_isEnable")]
        public bool SettingIsEnable { get; set; }
    }
}
