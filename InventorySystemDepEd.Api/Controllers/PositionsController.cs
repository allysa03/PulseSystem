using InventorySystemDepEd.Api.Data;
using InventorySystemDepEd.Api.Models;
using InventorySystemDepEd.Shared.DTOs;
using InventorySystemDepEd.Shared.DTOs.Positions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventorySystemDepEd.Api.Controllers;

[ApiController]
[Route("api/positions")]
public class PositionsController : ControllerBase
{
    private readonly AppDbContext _context;

    public PositionsController(AppDbContext context)
    {
        _context = context;
    }

    // GET ALL
    [HttpGet]
    public async Task<ActionResult<List<PositionDto>>> GetAll()
    {
        var list = await _context.Positions
            .OrderBy(p => p.PositionName)
            .Select(p => new PositionDto
            {
                PositionId = p.PositionId,
                PositionName = p.PositionName ?? ""
            })
            .ToListAsync();

        return Ok(list);
    }

    // GET BY ID
    [HttpGet("{id}")]
    public async Task<ActionResult<PositionDto>> GetById(int id)
    {
        var position = await _context.Positions
            .Where(p => p.PositionId == id)
            .Select(p => new PositionDto
            {
                PositionId = p.PositionId,
                PositionName = p.PositionName ?? ""
            })
            .FirstOrDefaultAsync();

        if (position == null)
            return NotFound();

        return Ok(position);
    }

    // POST
    [HttpPost]
    public async Task<IActionResult> Create(PositionDto dto)
    {
        var entity = new PositionsModel
        {
            PositionName = dto.PositionName
        };

        _context.Positions.Add(entity);
        await _context.SaveChangesAsync();

        return Ok();
    }

    // PUT
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, PositionDto dto)
    {
        var position = await _context.Positions.FindAsync(id);

        if (position == null)
            return NotFound();

        position.PositionName = dto.PositionName;

        await _context.SaveChangesAsync();

        return Ok();
    }

    // DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var position = await _context.Positions.FindAsync(id);

        if (position == null)
            return NotFound();

        _context.Positions.Remove(position);
        await _context.SaveChangesAsync();

        return Ok();
    }
}