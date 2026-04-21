using InventorySystemDepEd.Shared.Excel.Imports;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventorySystemDepEd.Shared.Excel
{
    public interface IExcelImportService
    {
        /// <summary>
        /// Imports Excel file into a raw row structure (dictionary-based)
        /// </summary>
        ExcelImportResult<Dictionary<string, string>> Import(Stream file);

        /// <summary>
        /// OPTIONAL: Strongly-typed import (for future DTO mapping)
        /// </summary>
        ExcelImportResult<T> Import<T>(
            Stream file,
            Func<Dictionary<string, string>, T> mapper);
    }
}
