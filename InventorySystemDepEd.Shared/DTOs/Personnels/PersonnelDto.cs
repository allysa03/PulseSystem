//using OfficeDto = InventorySystemDepEd.Shared.DTOs.Offices.OfficeDto;

namespace InventorySystemDepEd.Shared.DTOs.Personnels
{
    public class PersonnelDto
    {
        public int PersonnelId { get; set; }
        public string EmployeeID { get; set; } = string.Empty;

        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }

        public string? ContactNumber { get; set; }
        public string? EmailAddress { get; set; }

        public DateTime? HiredDate { get; set; }

        public int? OfficeId { get; set; }
        public int? PositionId { get; set; }
        public int? AccountID { get; set; }

        // computed (from joins)
        public string FullName { get; set; } = string.Empty;
        public string? PositionName { get; set; } = string.Empty;
        public string OfficeName { get; set; } = string.Empty;
        public OfficeDto? Office { get; set; }
    }
}