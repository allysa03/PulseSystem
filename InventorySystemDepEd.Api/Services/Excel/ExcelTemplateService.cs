namespace InventorySystemDepEd.Api.Services.Excel
{
    using ClosedXML.Excel;
    using InventorySystemDepEd.Shared.Excel;
    using InventorySystemDepEd.Shared.Excel.Definitions;

    public class ExcelTemplateService : IExcelTemplateService
    {
        public byte[] Generate(
            ExcelTemplateDefinition template,
            Dictionary<string, List<string>> dropdownData)
        {
            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add(template.SheetName);

            int colCount = template.Fields.Count;
            int lastRow = 1000;

            // ===============================
            // HEADER
            // ===============================
            foreach (var field in template.Fields)
            {
                ws.Cell(1, field.Order).Value = field.Header;
            }

            var header = ws.Range(1, 1, 1, colCount);

            header.Style.Font.Bold = true;
            header.Style.Font.FontColor = XLColor.White;
            header.Style.Fill.BackgroundColor = XLColor.DarkGreen;
            header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            header.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            ws.SheetView.FreezeRows(1);

            // ===============================
            // DROPDOWN SHEETS
            // ===============================
            foreach (var dd in dropdownData)
            {
                var sheet = wb.Worksheets.Add(dd.Key);

                for (int i = 0; i < dd.Value.Count; i++)
                    sheet.Cell(i + 1, 1).Value = dd.Value[i];

                sheet.Hide();
            }

            // ===============================
            // APPLY DROPDOWNS (SAFE VERSION)
            // ===============================
            foreach (var field in template.Fields)
            {
                if (!field.IsDropdown || string.IsNullOrWhiteSpace(field.LookupSheet))
                    continue;

                if (!dropdownData.TryGetValue(field.LookupSheet, out var list) || list == null || list.Count == 0)
                    continue;

                var range = $"={field.LookupSheet}!$A$1:$A${list.Count}";

                ws.Range(2, field.Order, lastRow, field.Order)
                  .CreateDataValidation()
                  .List(range);
            }

            // ===============================
            // TABLE + FORMAT
            // ===============================
            ws.Range(1, 1, lastRow, colCount).CreateTable();

            // Adjust columns to content with minimum width of 35
            ws.Columns().AdjustToContents();
            for (int col = 1; col <= colCount; col++)
            {
                if (ws.Column(col).Width < 35)
                    ws.Column(col).Width = 35;
            }

            // Format date columns (especially HiredDate) to be UTC now compatible
            foreach (var field in template.Fields)
            {
                if (field.Header.Contains("Hire", StringComparison.OrdinalIgnoreCase) || 
                    field.Header.Contains("Date", StringComparison.OrdinalIgnoreCase))
                {
                    // Apply date format to the entire column (rows 2 to lastRow)
                    var dateColumn = ws.Range(2, field.Order, lastRow, field.Order);
                    dateColumn.Style.DateFormat.Format = "yyyy-MM-dd HH:mm:ss";
                    dateColumn.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }
            }

            using var stream = new MemoryStream();
            wb.SaveAs(stream);
            return stream.ToArray();
        }
    }
}