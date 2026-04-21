using InventorySystemDepEd.Shared.Excel.Imports;

public class ExcelUploadSummary
{
    public int SuccessCount { get; set; }
    public int ErrorCount { get; set; }

    public List<ExcelRowError> MissingFields { get; set; } = new();
    public List<ExcelRowError> InvalidEmails { get; set; } = new();
    public List<ExcelRowError> InvalidDates { get; set; } = new();
    public List<ExcelRowError> InvalidReferences { get; set; } = new();
    public List<ExcelRowError> Duplicates { get; set; } = new();
}