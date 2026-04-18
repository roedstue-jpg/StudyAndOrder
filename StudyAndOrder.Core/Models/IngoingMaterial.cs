namespace StudyAndOrder.Core.Models
{
    public class IngoingMaterial
    {
        public int Id { get; set; }
        public string MaterialNumber { get; set; } = string.Empty;
        public string BatchNumber { get; set; } = string.Empty;
        public string Amount { get; set; } = string.Empty;

        // Foreign key
        public int OrderId { get; set; }

        // Navigation property
        public Order Order { get; set; } = null!;
    }
}