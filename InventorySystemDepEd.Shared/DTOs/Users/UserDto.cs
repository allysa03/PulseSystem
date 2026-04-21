namespace InventorySystemDepEd.Shared.DTOs.Users
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string UserEmailAddress { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public bool UserIsApproved { get; set; }
        public bool UserIsLocked { get; set; }
    }
}