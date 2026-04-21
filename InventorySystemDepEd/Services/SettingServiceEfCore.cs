//using InventorySystemDepEd.Data;
//using InventorySystemDepEd.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace InventorySystemDepEd.Services
{
    public class SettingServiceEfCore
    {
        //    private readonly AppDbContext _context;

        //    public SettingServiceEfCore(AppDbContext context)
        //    {
        //        _context = context;
        //    }

        //    public async Task<List<SettingsModel>> GetSettingsAsync()
        //    {
        //        return await _context.Settings.ToListAsync();
        //    }

        //    public async Task<SettingsModel?> GetByDocumentType(string docType)
        //    {
        //        return await _context.Settings
        //            .FirstOrDefaultAsync(x => x.SettingDocumentType == docType);
        //    }

        //    public async Task CreateSettingsAsync(SettingsModel model)
        //    {
        //        var existing = await _context.Settings
        //            .FirstOrDefaultAsync(x => x.SettingDocumentType == model.SettingDocumentType);

        //        if(existing == null)
        //        {
        //            _context.Settings.Add(model);
        //        }
        //        else
        //        {
        //            existing.SettingFormat = model.SettingFormat;
        //            existing.SettingIsEnable = model.SettingIsEnable;
        //        }

        //        await _context.SaveChangesAsync();
        //    }

        //    public async Task<string?> GeneratePreviewAsync(string docType)
        //    {
        //        var setting = await _context.Settings
        //            .FirstOrDefaultAsync(x => x.SettingDocumentType == docType);

        //        if (setting == null || string.IsNullOrEmpty(setting.SettingFormat))
        //            return null;

        //        var format = setting.SettingFormat;

        //        var now = DateTime.Now;
        //        var result = format;

        //        result = result.Replace("{YEAR}", now.Year.ToString());
        //        result = result.Replace("{MM}", now.Month.ToString("D2"));
        //        result = result.Replace("{DD}", now.Day.ToString("D2"));

        //        var match = Regex.Match(result, @"\{SEQ:(\d+)\}");

        //        if (match.Success)
        //        {
        //            int digits = int.Parse(match.Groups[1].Value);
        //            var seq = 1.ToString().PadLeft(digits, '0');

        //            result = Regex.Replace(result, @"\{SEQ:\d+\}", seq);
        //        }

        //        return result;
        //    }
    }
}
