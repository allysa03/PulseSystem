//using InventorySystemDepEd.Data;
//using InventorySystemDepEd.Models;
using Microsoft.EntityFrameworkCore;

namespace InventorySystemDepEd.Services
{
    public class PositionServiceEfCore
    {
        //private readonly AppDbContext _context;

        //public PositionServiceEfCore(AppDbContext context)
        //{
        //    _context = context;
        //}

        //// Fetch all positions
        //public async Task<List<PositionsModel>> GetAllPositionsAsync()
        //{
        //    return await _context.Positions
        //                         .AsNoTracking()
        //                         .ToListAsync();
        //}

        //// Optional: Fetch a single position by ID
        //public async Task<PositionsModel?> GetPositionByIdAsync(int id)
        //{
        //    return await _context.Positions
        //                         .AsNoTracking()
        //                         .FirstOrDefaultAsync(p => p.PositionId == id);
        //}
    }
}