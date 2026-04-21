using InventorySystemDepEd.Api.Data;
using InventorySystemDepEd.Api.Models;
using InventorySystemDepEd.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventorySystemDepEd.Api.Controllers;

[ApiController]
[Route("api/suppliers")]
public class SuppliersController : ControllerBase
{
    private readonly AppDbContext _context;

    public SuppliersController(AppDbContext context)
    {
        _context = context;
    }

    // GET ALL
    [HttpGet]
    public async Task<ActionResult<List<SupplierDto>>> GetAll()
    {
        var list = await _context.Suppliers
            .OrderBy(s => s.SupplierName)
            .Select(s => new SupplierDto
            {
                SupplierId = s.SupplierId, // ✅ FIX HERE
                SupplierName = s.SupplierName,
                SupplierAddress = s.SupplierAddress,
                SupplierContactNumber = s.SupplierContactNumber,
                SupplierEmailAddress = s.SupplierEmailAddress,
                SupplierTinNumber = s.SupplierTinNumber
            })
            .ToListAsync();

        return Ok(list);
    }

    // GET BY ID
    [HttpGet("{id}")]
    public async Task<ActionResult<SupplierDto>> GetById(int id)
    {
        var supplier = await _context.Suppliers
            .Where(s => s.SupplierId == id)
            .Select(s => new SupplierDto
            {
                SupplierId = s.SupplierId, // ✅ FIX HERE
                SupplierName = s.SupplierName,
                SupplierAddress = s.SupplierAddress,
                SupplierContactNumber = s.SupplierContactNumber,
                SupplierEmailAddress = s.SupplierEmailAddress,
                SupplierTinNumber = s.SupplierTinNumber
            })
            .FirstOrDefaultAsync();

        if (supplier == null)
            return NotFound();

        return Ok(supplier);
    }

    // POST
    [HttpPost]
    public async Task<IActionResult> Create(SupplierDto dto)
    {
        var entity = new SuppliersModel
        {
            SupplierName = dto.SupplierName,
            SupplierAddress = dto.SupplierAddress,
            SupplierContactNumber = dto.SupplierContactNumber,
            SupplierEmailAddress = dto.SupplierEmailAddress,
            SupplierTinNumber = dto.SupplierTinNumber
        };

        _context.Suppliers.Add(entity);
        await _context.SaveChangesAsync();

        return Ok();
    }

    // PUT
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, SupplierDto dto)
    {
        var supplier = await _context.Suppliers.FindAsync(id);

        if (supplier == null)
            return NotFound();

        supplier.SupplierName = dto.SupplierName;
        supplier.SupplierAddress = dto.SupplierAddress;
        supplier.SupplierContactNumber = dto.SupplierContactNumber;
        supplier.SupplierEmailAddress = dto.SupplierEmailAddress;
        supplier.SupplierTinNumber = dto.SupplierTinNumber;

        await _context.SaveChangesAsync();

        return Ok();
    }

    // DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var supplier = await _context.Suppliers.FindAsync(id);

        if (supplier == null)
            return NotFound();

        _context.Suppliers.Remove(supplier);
        await _context.SaveChangesAsync();

        return Ok();
    }
}