using InventorySystemDepEd.Shared.Excel.Definitions;

namespace InventorySystemDepEd.Shared.Excel
{
    public interface IExcelTemplateRegistry
    {
        /// <summary>
        /// Get a specific template by name
        /// </summary>
        ExcelTemplateDefinition Get(string name);

        /// <summary>
        /// Get dropdown data for a template
        /// </summary>
        Dictionary<string, List<string>> GetDropdowns(string name);

        /// <summary>
        /// Get all available templates
        /// </summary>
        List<ExcelTemplateInfo> GetAll();
    }
}