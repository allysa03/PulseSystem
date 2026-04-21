namespace InventorySystemDepEd.Shared.Excel.Definitions
{
    public class ExcelTemplateDefinition
    {
        // Sheet name in Excel file
        public string SheetName { get; set; } = "";

        // Column definitions
        public List<ExcelFieldDefinition> Fields { get; set; } = new();

        // 🔥 NEW: template identifier (used by registry)
        public string Name { get; set; } = "";

        // 🔥 NEW: display name (for UI dropdown/buttons)
        public string DisplayName { get; set; } = "";

        // 🔥 NEW: optional versioning (future-proof)
        public int Version { get; set; } = 1;

        // 🔥 NEW: optional description (UI help)
        public string? Description { get; set; }
    }
}