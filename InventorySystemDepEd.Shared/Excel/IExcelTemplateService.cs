using InventorySystemDepEd.Shared.Excel.Definitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventorySystemDepEd.Shared.Excel
{
    public interface IExcelTemplateService
    {
        /// <summary>
        /// Generates an Excel template file (byte array)
        /// </summary>
        byte[] Generate(
            ExcelTemplateDefinition template,
            Dictionary<string, List<string>> dropdownData);
    }
}
