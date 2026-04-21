using InventorySystemDepEd.Api.Data;

public class OfficeProvider
{
    private readonly AppDbContext _context;

    public OfficeProvider(AppDbContext context)
    {
        _context = context;
    }

    public List<string> GetOffices()
    {
        return _context.Offices
            .Where(x => !string.IsNullOrWhiteSpace(x.OfficeName))
            .Select(x => x.OfficeName!)
            .ToList();
    }
}