//using InventorySystemDepEd.Data;
//using InventorySystemDepEd.Models;
using Microsoft.EntityFrameworkCore;

namespace InventorySystemDepEd.Services
{
    public class LogoServiceEfCore
    {
        //private readonly AppDbContext _context;

        //public LogoServiceEfCore(AppDbContext context)
        //{
        //    _context = context;
        //}

        //// GET LOGO BY TYPE
        //public async Task<LogoModel?> GetLogoAsync(string logoType)
        //{
        //    return await _context.Set<LogoModel>()
        //        .FirstOrDefaultAsync(x => x.LogoType == logoType);
        //}

        //// GET ALL LOGOS
        //public async Task<List<LogoModel>> GetLogosAsync()
        //{
        //    return await _context.Set<LogoModel>().ToListAsync();
        //}

        //// CREATE OR UPDATE LOGO
        //public async Task SaveLogoAsync(string logoType, byte[] imageData, string contentType)
        //{
        //    var existing = await _context.Set<LogoModel>()
        //        .FirstOrDefaultAsync(x => x.LogoType == logoType);

        //    if (existing == null)
        //    {
        //        var newLogo = new LogoModel
        //        {
        //            LogoType = logoType,
        //            ImageData = imageData,
        //            ContentType = contentType
        //        };

        //        _context.Add(newLogo);
        //    }
        //    else
        //    {
        //        existing.ImageData = imageData;
        //        existing.ContentType = contentType;
        //    }

        //    await _context.SaveChangesAsync();
        //}

        //// DELETE LOGO
        //public async Task DeleteLogoAsync(string logoType)
        //{
        //    var logo = await _context.Set<LogoModel>()
        //        .FirstOrDefaultAsync(x => x.LogoType == logoType);

        //    if (logo != null)
        //    {
        //        _context.Remove(logo);
        //        await _context.SaveChangesAsync();
        //    }
        //}
    }
}