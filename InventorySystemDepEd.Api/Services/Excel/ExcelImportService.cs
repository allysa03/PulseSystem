namespace InventorySystemDepEd.Api.Services.Excel
{
    using ClosedXML.Excel;
    using InventorySystemDepEd.Shared.Excel;
    using InventorySystemDepEd.Shared.Excel.Imports;

    public class ExcelImportService : IExcelImportService
    {
        public ExcelImportResult<Dictionary<string, string>> Import(Stream file)
        {
            using var wb = new XLWorkbook(file);
            var ws = wb.Worksheet(1);

            var result = new ExcelImportResult<Dictionary<string, string>>();

            var lastRow = ws.LastRowUsed()?.RowNumber() ?? 1;
            var lastCol = ws.LastColumnUsed()?.ColumnNumber() ?? 1;

            // ===============================
            // READ HEADERS
            // ===============================
            var headers = new List<string>();

            for (int col = 1; col <= lastCol; col++)
            {
                var header = ws.Cell(1, col).GetString();
                headers.Add(header);
            }

            // ===============================
            // READ ROWS
            // ===============================
            for (int row = 2; row <= lastRow; row++)
            {
                var rowData = new Dictionary<string, string>();
                bool hasData = false;

                for (int col = 1; col <= lastCol; col++)
                {
                    var value = ws.Cell(row, col).GetString();

                    if (!string.IsNullOrWhiteSpace(value))
                        hasData = true;

                    var key = headers[col - 1];

                    if (!string.IsNullOrWhiteSpace(key))
                        rowData[key] = value;
                }

                if (!hasData)
                    continue;

                result.Data.Add(rowData);
            }

            return result;
        }

        public ExcelImportResult<T> Import<T>(
            Stream file,
            Func<Dictionary<string, string>, T> mapper)
        {
            var rawResult = Import(file);
            var result = new ExcelImportResult<T>();

            foreach (var rowData in rawResult.Data)
            {
                try
                {
                    var mappedItem = mapper(rowData);
                    result.Data.Add(mappedItem);
                }
                catch (Exception ex)
                {
                    result.Errors.Add(new ExcelRowError
                    {
                        RowNumber = result.Data.Count + 2,
                        Column = "General",
                        Message = $"Mapping error: {ex.Message}"
                    });
                }
            }

            result.Errors.AddRange(rawResult.Errors);

            return result;
        }
    }
}