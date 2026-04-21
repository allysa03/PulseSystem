namespace InventorySystemDepEd.Shared.DTOs
{
    public class TransferActionDto
    {
        public int PersonnelId { get; set; }

        // "Approved" or "Rejected"
        public string Action { get; set; } = string.Empty;

        public string? Remark { get; set; }

        public int UserId { get; set; }
    }
}