namespace InventorySystemDepEd.Shared.DTOs
{
    public class DepartmentDto
    {
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; } = string.Empty;

        public string? ContactNumber { get; set; }

        public string? Location { get; set; }

        public int? Head { get; set; }

        // ================= UI / READ-ONLY FIELDS =================

        public string? HeadName { get; set; }

        public int OfficeCount { get; set; }
    }
}