using System;
using System.Collections.Generic;
using System.Text;

namespace InventorySystemDepEd.Shared.Excel.Imports
{
    public class ExcelImportResult<T>
    {
        public List<T> Data { get; set; } = new();
        public List<ExcelRowError> Errors { get; set; } = new();
        public bool HasErrors => Errors.Any();
    }
}
