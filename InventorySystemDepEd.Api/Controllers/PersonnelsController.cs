using InventorySystemDepEd.Api.Data;
using InventorySystemDepEd.Api.Helpers;
using InventorySystemDepEd.Api.Models;
using InventorySystemDepEd.Api.Services;
using InventorySystemDepEd.Api.Services.Excel;
using InventorySystemDepEd.Shared.DTOs;
using InventorySystemDepEd.Shared.DTOs.Personnels;
using InventorySystemDepEd.Shared.DTOs.Positions;
using InventorySystemDepEd.Shared.Excel.Imports;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

#nullable enable

namespace InventorySystemDepEd.Api.Controllers;

[ApiController]
[Route("api/personnels")]
public class PersonnelsController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    // =========================
    // GET ALL
    // =========================
    [HttpGet]
    public async Task<ActionResult<List<PersonnelDto>>> GetAll()
    {
        try
        {
            var list = await _context.Personnels
                .Include(p => p.Position)
                .Include(p => p.Office)
                .OrderBy(p => p.LastName)
                .Select(p => new PersonnelDto
                {
                    PersonnelId = p.PersonnelId,
                    EmployeeID = p.EmployeeID ?? "",
                    FirstName = p.FirstName ?? "",
                    MiddleName = p.MiddleName ?? "",
                    LastName = p.LastName ?? "",
                    FullName = (p.LastName ?? "") + ", " + (p.FirstName ?? ""),
                    PositionId = p.PositionId,
                    PositionName = p.Position != null ? p.Position.PositionName : "",
                    OfficeId = p.OfficeId,
                    OfficeName = p.Office != null ? (p.Office.OfficeName ?? "") : "",
                    EmailAddress = p.EmailAddress ?? "",
                    ContactNumber = p.ContactNumber ?? "",
                    HiredDate = p.HiredDate
                })
                .ToListAsync();

            return Ok(list);
        }
        catch (Exception ex)
        {
            // Log the actual error for debugging
            Console.WriteLine($"Error in GetAll: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    // =========================
    // GET BY ID
    // =========================
    [HttpGet("{id}")]
    public async Task<ActionResult<PersonnelDto>> GetById(int id)
    {
        var p = await _context.Personnels
            .Include(x => x.Position)
            .Include(x => x.Office)
            .Where(x => x.PersonnelId == id)
            .Select(x => new PersonnelDto
            {
                PersonnelId = x.PersonnelId,
                EmployeeID = x.EmployeeID ?? "",
                FirstName = x.FirstName ?? "",
                MiddleName = x.MiddleName ?? "",
                LastName = x.LastName ?? "",
                PositionId = x.PositionId,
                PositionName = x.Position != null ? x.Position.PositionName : "",
                OfficeId = x.OfficeId,
                OfficeName = x.Office != null ? (x.Office.OfficeName ?? "") : "",
                EmailAddress = x.EmailAddress ?? "",
                ContactNumber = x.ContactNumber ?? "",
                HiredDate = x.HiredDate
            })
            .FirstOrDefaultAsync();

        return p == null ? NotFound() : Ok(p);
    }

    // =========================
    // CREATE
    // =========================
    [HttpPost]
    public async Task<IActionResult> Create(PersonnelDto dto)
    {
        var entity = new PersonnelsModel
        {
            EmployeeID = dto.EmployeeID,
            FirstName = StringHelper.ToProperCase(dto.FirstName),
            MiddleName = StringHelper.ToProperCase(dto.MiddleName),
            LastName = StringHelper.ToProperCase(dto.LastName),
            EmailAddress = dto.EmailAddress,
            ContactNumber = dto.ContactNumber,
            HiredDate = dto.HiredDate.HasValue
                ? DateTime.SpecifyKind(dto.HiredDate.Value, DateTimeKind.Utc)
                : null,
            OfficeId = dto.OfficeId,
            PositionId = dto.PositionId
        };

        _context.Personnels.Add(entity);
        await _context.SaveChangesAsync();

        return Ok();
    }

    // =========================
    // UPDATE
    // =========================
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, PersonnelDto dto)
    {
        var entity = await _context.Personnels.FindAsync(id);
        if (entity == null) return NotFound();

        entity.EmployeeID = dto.EmployeeID;
        entity.FirstName = StringHelper.ToProperCase(dto.FirstName);
        entity.MiddleName = StringHelper.ToProperCase(dto.MiddleName);
        entity.LastName = StringHelper.ToProperCase(dto.LastName);
        entity.EmailAddress = dto.EmailAddress;
        entity.ContactNumber = dto.ContactNumber;
        entity.HiredDate = dto.HiredDate.HasValue
            ? DateTime.SpecifyKind(dto.HiredDate.Value, DateTimeKind.Utc)
            : null;
        entity.OfficeId = dto.OfficeId;
        entity.PositionId = dto.PositionId;

        await _context.SaveChangesAsync();
        return Ok();
    }

    // =========================
    // DELETE
    // =========================
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _context.Personnels.FindAsync(id);
        if (entity == null) return NotFound();

        _context.Personnels.Remove(entity);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ExcelUploadSummary>> UploadExcel(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "File is required" });

        if (!file.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
            return BadRequest(new { message = "Only .xlsx files are supported" });

        try
        {
            var summary = new ExcelUploadSummary();

            // -------------------------
            // LOOKUPS
            // -------------------------
            var positions = await _context.Positions.AsNoTracking().ToListAsync();
            var offices = await _context.Offices.AsNoTracking().ToListAsync();

            var positionDict = positions.ToDictionary(x => x.PositionName?.ToLower() ?? "");
            var officeDict = offices.ToDictionary(x => x.OfficeName?.ToLower() ?? "");

            // -------------------------
            // READ EXCEL
            // -------------------------
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            var importService = new ExcelImportService();
            var rawImportResult = importService.Import(stream);

            var validData = new List<PersonnelsModel>();
            var errors = new List<ExcelRowError>();

            int rowNum = 2;

            // -------------------------
            // VALIDATION
            // -------------------------
            foreach (var row in rawImportResult.Data)
            {
                var employeeId = row.GetValueOrDefault("EmployeeID")?.Trim();
                var firstName = row.GetValueOrDefault("FirstName")?.Trim();
                var lastName = row.GetValueOrDefault("LastName")?.Trim();
                var email = row.GetValueOrDefault("EmailAddress")?.Trim();
                var positionName = row.GetValueOrDefault("Position")?.Trim();
                var officeName = row.GetValueOrDefault("Office")?.Trim();
                var hiredDateStr = row.GetValueOrDefault("HiredDate")?.Trim();

                var fullName = $"{firstName} {lastName}".Trim();
                var rowErrors = new List<ExcelRowError>();

                // REQUIRED
                if (string.IsNullOrWhiteSpace(employeeId))
                    rowErrors.Add(new ExcelRowError(rowNum, "EmployeeID", "Required", employeeId ?? "", fullName));

                if (string.IsNullOrWhiteSpace(firstName))
                    rowErrors.Add(new ExcelRowError(rowNum, "FirstName", "Required", employeeId ?? "", fullName));

                if (string.IsNullOrWhiteSpace(lastName))
                    rowErrors.Add(new ExcelRowError(rowNum, "LastName", "Required", employeeId ?? "", fullName));

                // EMAIL
                if (string.IsNullOrWhiteSpace(email))
                    rowErrors.Add(new ExcelRowError(rowNum, "EmailAddress", "Required", employeeId ?? "", fullName));
                else if (!email.EndsWith("@deped.gov.ph", StringComparison.OrdinalIgnoreCase))
                    rowErrors.Add(new ExcelRowError(rowNum, "EmailAddress", "Invalid Email Address", employeeId ?? "", fullName));

                // LOOKUPS
                positionDict.TryGetValue(positionName?.ToLower() ?? "", out var position);
                officeDict.TryGetValue(officeName?.ToLower() ?? "", out var office);

                if (position == null)
                    rowErrors.Add(new ExcelRowError(rowNum, "Position", "Not found", employeeId ?? "", fullName));

                if (office == null)
                    rowErrors.Add(new ExcelRowError(rowNum, "Office", "Not found", employeeId ?? "", fullName));

                // DATE
                DateTime? hiredDate = null;
                if (!DateTime.TryParse(hiredDateStr, out var parsed))
                {
                    rowErrors.Add(new ExcelRowError(rowNum, "HiredDate", "Invalid date", employeeId ?? "", fullName));
                }
                else
                {
                    hiredDate = DateTime.SpecifyKind(parsed, DateTimeKind.Utc);
                }

                // -------------------------
                // RESULT
                // -------------------------
                if (rowErrors.Any())
                {
                    errors.AddRange(rowErrors);
                }
                else
                {
                    validData.Add(new PersonnelsModel
                    {
                        EmployeeID = employeeId!,
                        FirstName = StringHelper.ToProperCase(firstName),
                        LastName = StringHelper.ToProperCase(lastName),
                        MiddleName = row.GetValueOrDefault("MiddleName"),
                        EmailAddress = email,
                        ContactNumber = row.GetValueOrDefault("ContactNumber"),
                        PositionId = position!.PositionId,
                        OfficeId = office!.OfficeId,
                        HiredDate = hiredDate
                    });
                }

                rowNum++;
            }

            // -------------------------
            // DUPLICATE CHECK
            // -------------------------
            var existingPersonnels = await _context.Personnels.AsNoTracking().ToListAsync();

            var existingDict = existingPersonnels
                .ToDictionary(x => x.EmployeeID?.ToLower() ?? "");

            var finalInsert = new List<PersonnelsModel>();

            foreach (var item in validData)
            {
                var key = item.EmployeeID?.ToLower() ?? "";

                if (existingDict.ContainsKey(key))
                {
                    var existing = existingDict[key];

                    summary.Duplicates.Add(new ExcelRowError
                    {
                        RowNumber = rowNum,
                        EmployeeID = item.EmployeeID = "",
                        FullName = $"{item.FirstName} {item.LastName}",
                        Column = "EmployeeID",
                        Message = $"Duplicate of existing record"
                    });
                }
                else
                {
                    finalInsert.Add(item);
                }
            }

            // -------------------------
            // SAVE ONLY VALID + NON-DUPLICATES
            // -------------------------
            if (finalInsert.Any())
            {
                await _context.Personnels.AddRangeAsync(finalInsert);
                await _context.SaveChangesAsync();
            }

            // -------------------------
            // SUMMARY
            // -------------------------
            summary.SuccessCount = finalInsert.Count;
            summary.ErrorCount = errors.Count;

            summary.MissingFields = errors
                .Where(e => e.Message == "Required")
                .ToList();

            summary.InvalidEmails = errors
                .Where(e => e.Column == "EmailAddress")
                .ToList();

            summary.InvalidDates = errors
                .Where(e => e.Column == "HiredDate")
                .ToList();

            summary.InvalidReferences = errors
                .Where(e => e.Column is "Position" or "Office")
                .ToList();

            return Ok(summary);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = ex.InnerException?.Message ?? ex.Message,
                full = ex.ToString()
            });
        }
    }

}