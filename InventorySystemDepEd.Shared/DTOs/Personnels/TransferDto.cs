namespace InventorySystemDepEd.Shared.DTOs
{
    public class TransferDto
    {
        // Personnel Info
        public int PersonnelId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PositionName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public DateTime? HiredDate { get; set; }

        // Transfer Info
        public int OriginOfficeId { get; set; }
        public int DestinationOfficeId { get; set; }
        public string? Remark { get; set; }
        public string? RejectRemark { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime? TransferDate { get; set; }
        public string? DestinationOfficeName { get; set; }
    }
}