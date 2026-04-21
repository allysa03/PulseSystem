namespace InventorySystemDepEd.Shared.DTOs.Auth
{
    public class RegisterRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Boolean TermsAndConditions { get; set; } = false;
    }
}