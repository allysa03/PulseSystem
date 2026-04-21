using System;
using System.Collections.Generic;
using System.Text;

namespace InventorySystemDepEd.Shared.Excel.Imports
{
    public class ExcelRowError
    {
        public int RowNumber { get; set; }
        public string Column { get; set; } = "";
        public string Message { get; set; } = "";

        // NEW: add identity info
        public string EmployeeID { get; set; } = "";
        public string FullName { get; set; } = "";

        public ExcelRowError() { }

        public ExcelRowError(
         int rowNumber,
         string column,
         string message,
         string employeeId = "",
         string fullName = "")
        {
            RowNumber = rowNumber;
            Column = column;
            Message = message;
            EmployeeID = employeeId;
            FullName = fullName;
        }
    }
}
