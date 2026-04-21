using PersonnelDto = InventorySystemDepEd.Shared.DTOs.Personnels.PersonnelDto;

namespace InventorySystemDepEd.Shared.DTOs.Profile
{
    public class UserProfileDto
    {
        public int UserId { get; set; }

        public string UserEmailAddress { get; set; } = string.Empty;

        public bool UserIsApproved { get; set; }

        public string RoleName { get; set; } = string.Empty;

        public PersonnelDto Personnel { get; set; } = new();
    }

    //public class PersonnelDto
    //{
    //    public int PersonnelId { get; set; }

    //    public string EmployeeID { get; set; } = string.Empty;

    //    public string FirstName { get; set; } = string.Empty;

    //    public string MiddleName { get; set; } = string.Empty;

    //    public string LastName { get; set; } = string.Empty;

    //    public string ContactNumber { get; set; } = string.Empty;

    //    public DateTime? HiredDate { get; set; }

    //    public string PositionName { get; set; } = string.Empty;

    //    public OfficeDto? Office { get; set; }
    //}

    //public class OfficeDto
    //{
    //    public string OfficeName { get; set; } = string.Empty;

    //    public DepartmentDto? Department { get; set; }
    //}

    //public class DepartmentDto
    //{
    //    public string DepartmentName { get; set; } = string.Empty;
    //}
}