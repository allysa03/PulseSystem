namespace InventorySystemDepEd.Shared.DTOs
{
    public class SupplierDto
    {
        public int SupplierId { get; set; } 
        public string SupplierName { get; set; } = null!;
        public string? SupplierAddress { get; set; }
        public string? SupplierContactNumber { get; set; }
        public string? SupplierEmailAddress { get; set; }
        public string? SupplierTinNumber { get; set; }
    }
}