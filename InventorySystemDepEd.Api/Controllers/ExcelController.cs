using Microsoft.AspNetCore.Mvc;
using InventorySystemDepEd.Api.Services.Excel;
using InventorySystemDepEd.Shared.Excel;


namespace InventorySystemDepEd.Api.Controllers
{
    [ApiController]
    [Route("api/excel")]
    public class ExcelController : ControllerBase
    {
        private readonly IExcelTemplateService _excelService;
        private readonly IExcelTemplateRegistry _registry;

        private const string ExcelContentType =
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public ExcelController(
            IExcelTemplateService excelService,
            IExcelTemplateRegistry registry)
        {
            _excelService = excelService;
            _registry = registry;
        }

        /// <summary>
        /// Download any Excel template dynamically
        /// Example: /api/excel/template/personnel
        /// </summary>
        [HttpGet("template/{name}")]
        public IActionResult DownloadTemplate(string name)
        {
            try
            {
                var template = _registry.Get(name);

                if (template == null)
                    return NotFound($"Template '{name}' not found.");

                var dropdowns = _registry.GetDropdowns(name);

                var fileBytes = _excelService.Generate(template, dropdowns);

                return File(
                    fileBytes,
                    ExcelContentType,
                    $"{name}Template.xlsx"
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to generate Excel template", details = ex.Message });
            }
        }

        /// <summary>
        /// Optional: list all available templates (for your Blazor UI)
        /// </summary>
        [HttpGet("templates")]
        public IActionResult GetTemplates()
        {
            var templates = _registry.GetAll();

            return Ok(templates.Select(t => new
            {
                name = t.Name,
                displayName = t.DisplayName
            }));
        }
    }
}