using InventorySystemDepEd.Api.Data;
using InventorySystemDepEd.Api.Models;
using InventorySystemDepEd.Shared.DTOs;
using InventorySystemDepEd.Shared.DTOs.Personnels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventorySystemDepEd.Api.Controllers;

[ApiController]
[Route("api/offices")]
public class OfficesController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    // ======================================================
    // OFFICES
    // ======================================================

    [HttpGet]
    public async Task<ActionResult<List<OfficeDto>>> GetAll()
    {
        var offices = await _context.Offices
            .Include(o => o.Department)
            .ToListAsync();

        var result = offices.Select(o => new OfficeDto
        {
            OfficeId = o.OfficeId,
            OfficeName = o.OfficeName ?? "",
            ContactNumber = o.ContactNumber ?? "",
            Location = o.Location ?? "",
            Head = o.Head,
            DepartmentId = o.DepartmentId,
            DepartmentName = o.Department?.DepartmentName ?? ""
        }).ToList();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OfficeDto>> GetById(int id)
    {
        var office = await _context.Offices
            .Include(o => o.HeadPersonnel)
            .Include(o => o.Department!)
                .ThenInclude(d => d.HeadPersonnel)
            .FirstOrDefaultAsync(o => o.OfficeId == id);

        if (office == null)
            return NotFound();

        var dto = new OfficeDto
        {
            OfficeId = office.OfficeId,
            OfficeName = office.OfficeName ?? "",
            ContactNumber = office.ContactNumber ?? "",
            Location = office.Location ?? "",
            Head = office.Head,
            DepartmentId = office.DepartmentId,

            // ✅ Office Head
            OfficeHeadName = office.HeadPersonnel?.FullName,

            // ✅ Department
            DepartmentName = office.Department?.DepartmentName ?? "",

            // ✅ Department Head
            DepartmentHeadName = office.Department?.HeadPersonnel?.FullName
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create(OfficeDto dto)
    {
        var entity = new OfficesModel
        {
            OfficeName = dto.OfficeName ?? "",
            DepartmentId = dto.DepartmentId,
            Head = dto.Head,
            Location = dto.Location ?? "",
            ContactNumber = dto.ContactNumber ?? ""
        };

        _context.Offices.Add(entity);
        await _context.SaveChangesAsync();

        dto.OfficeId = entity.OfficeId;

        return Ok(dto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, OfficeDto dto)
    {
        var entity = await _context.Offices.FindAsync(id);
        if (entity == null) return NotFound();

        entity.OfficeName = dto.OfficeName ?? "";
        entity.DepartmentId = dto.DepartmentId;
        entity.Head = dto.Head;
        entity.Location = dto.Location ?? "";
        entity.ContactNumber = dto.ContactNumber ?? "";

        await _context.SaveChangesAsync();

        return Ok(dto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _context.Offices.FindAsync(id);
        if (entity == null) return NotFound();

        _context.Offices.Remove(entity);
        await _context.SaveChangesAsync();

        return Ok();
    }

    // ======================================================
    // DEPARTMENTS (FIXED - DTO ONLY)
    // ======================================================

    [HttpGet("departments")]
    public async Task<ActionResult<List<DepartmentDto>>> GetDepartments()
    {
        var list = await _context.Departments
            .OrderBy(d => d.DepartmentName)
            .Select(d => new DepartmentDto
            {
                DepartmentId = d.DepartmentId,
                DepartmentName = d.DepartmentName ?? ""
            })
            .ToListAsync();

        return Ok(list);
    }

    [HttpGet("departments/{id}")]
    public async Task<ActionResult<DepartmentDto>> GetDepartmentById(int id)
    {
        var dept = await _context.Departments
            .FirstOrDefaultAsync(d => d.DepartmentId == id);

        if (dept == null) return NotFound();

        return Ok(new DepartmentDto
        {
            DepartmentId = dept.DepartmentId,
            DepartmentName = dept.DepartmentName ?? ""
        });
    }

    [HttpPost("departments")]
    public async Task<IActionResult> CreateDepartment(DepartmentDto dto)
    {
        var entity = new DepartmentsModel
        {
            DepartmentName = dto.DepartmentName ?? ""
        };

        _context.Departments.Add(entity);
        await _context.SaveChangesAsync();

        dto.DepartmentId = entity.DepartmentId;

        return Ok(dto);
    }

    [HttpPut("departments/{id}")]
    public async Task<IActionResult> UpdateDepartment(int id, DepartmentDto dto)
    {
        var entity = await _context.Departments.FindAsync(id);
        if (entity == null) return NotFound();

        entity.DepartmentName = dto.DepartmentName ?? "";

        await _context.SaveChangesAsync();

        return Ok(dto);
    }

    [HttpDelete("departments/{id}")]
    public async Task<IActionResult> DeleteDepartment(int id)
    {
        var entity = await _context.Departments.FindAsync(id);
        if (entity == null) return NotFound();

        _context.Departments.Remove(entity);
        await _context.SaveChangesAsync();

        return Ok();
    }

    // ======================================================
    // PERSONNELS (FIXED - DTO ONLY)
    // ======================================================

    [HttpGet("personnels/available")]
    public async Task<ActionResult<List<PersonnelDto>>> GetAvailablePersonnels([FromQuery] int? officeId)
    {
        var transferredIds = await _context.PersonnelTransfers
            .Where(t => t.Status == "Pending")
            .Select(t => t.PersonnelId)
            .ToHashSetAsync();

        var query = _context.Personnels
            .Include(p => p.Position)
            .AsQueryable();

        if (officeId.HasValue)
            query = query.Where(p => p.OfficeId == officeId);

        var result = await query
            .Where(p => !transferredIds.Contains(p.PersonnelId))
            .Select(p => new PersonnelDto
            {
                PersonnelId = p.PersonnelId,
                FullName = (p.FirstName ?? "") + " " + (p.LastName ?? ""),
                PositionName = p.Position != null ? p.Position.PositionName : null,
                EmailAddress = p.EmailAddress ?? "",
                ContactNumber = p.ContactNumber ?? "",
                OfficeId = p.OfficeId
            })
            .ToListAsync();

        return Ok(result);
    }

    [HttpGet("{officeId}/personnels")]
    public async Task<ActionResult<List<PersonnelDto>>> GetOfficePersonnels(int officeId)
    {
        var personnels = await _context.Personnels
            .Include(p => p.Position)
            .Where(p => p.OfficeId == officeId)
            .ToListAsync();

        var result = personnels.Select(p => new PersonnelDto
        {
            PersonnelId = p.PersonnelId,
            FullName = $"{p.FirstName ?? ""} {p.LastName ?? ""}",
            PositionName = p.Position != null ? p.Position.PositionName : "",
            EmailAddress = p.EmailAddress ?? "",
            ContactNumber = p.ContactNumber ?? "",
            HiredDate = p.HiredDate
        }).ToList();

        return Ok(result);
    }

    // ======================================================
    // TRANSFERS (ALREADY SAFE)
    // ======================================================

    [HttpGet("{officeId}/transfers")]
    public async Task<ActionResult<List<TransferDto>>> GetTransferred(int officeId)
    {
        var result = await (
            from t in _context.PersonnelTransfers
            join p in _context.Personnels on t.PersonnelId equals p.PersonnelId
            join pos in _context.Positions on p.PositionId equals pos.PositionId
            join dest in _context.Offices on t.DestinationOfficeId equals dest.OfficeId
            where t.OriginOfficeId == officeId
            || t.DestinationOfficeId == officeId
            select new TransferDto
            {
                PersonnelId = p.PersonnelId,
                FullName = (p.FirstName ?? "") + " " + (p.LastName ?? ""),
                PositionName = pos.PositionName ?? "",
                OriginOfficeId = t.OriginOfficeId,
                DestinationOfficeId = t.DestinationOfficeId,
                DestinationOfficeName = dest.OfficeName ?? "",
                Remark = t.Remark,
                RejectRemark = t.RejectRemark,
                Status = t.Status,
                TransferDate = t.TransferDate
            }
        ).ToListAsync();

        return Ok(result);
    }
}