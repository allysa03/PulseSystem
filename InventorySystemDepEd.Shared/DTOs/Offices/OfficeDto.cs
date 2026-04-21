namespace InventorySystemDepEd.Shared.DTOs
{
    public class OfficeDto
    {
        public int OfficeId { get; set; }
        public string? OfficeName { get; set; }
        public string? ContactNumber { get; set; }
        public string? Location { get; set; }

        public int? Head { get; set; }
        public string? OfficeHeadName { get; set; }

        public int? DepartmentId { get; set; }

        // ✅ DISPLAY FIELDS
        public string? DepartmentName { get; set; }
        public string? DepartmentHeadName { get; set; }
    }
}