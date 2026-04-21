namespace InventorySystemDepEd.Shared.DTOs
{
    public class TransferRequestDto
    {
        public List<int> PersonnelIds { get; set; } = new();

        public int OriginOfficeId { get; set; }

        public int DestinationOfficeId { get; set; }

        public int CreatedBy { get; set; }

        public string? Remark { get; set; }
    }
}