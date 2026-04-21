using System;
using System.Collections.Generic;
using System.Text;

namespace InventorySystemDepEd.Shared.Excel.Definitions
{
    public class ExcelFieldDefinition
    {
        public string Header { get; set; } = "";
        public int Order { get; set; }

        public bool IsRequired { get; set; }
        public bool IsDropdown { get; set; }

        public string? LookupSheet { get; set; }
        public string PropertyName { get; set; } = "";
    }
}
