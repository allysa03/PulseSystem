using InventorySystemDepEd.Api.Data;

public class PositionProvider
{
    private readonly AppDbContext _context;

    public PositionProvider(AppDbContext context)
    {
        _context = context;
    }

    public List<string> GetPositions()
    {
        return _context.Positions
            .Where(x => !string.IsNullOrWhiteSpace(x.PositionName))
            .Select(x => x.PositionName!)
            .ToList();
    }
}