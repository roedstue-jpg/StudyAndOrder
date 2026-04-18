using StudyAndOrder.Core.Enums;

namespace StudyAndOrder.Core.Models
{
    public class Study
    {
        public int Id { get; set; }
        public string StudyId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string WBS { get; set; } = string.Empty;
        public string CostCenter { get; set; } = string.Empty;
        public string Facility { get; set; } = string.Empty;
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public ProcessOrderType ProcessOrderType { get; set; }
        public bool DataEquipmentValidation { get; set; }
        public bool SamplingStock { get; set; }

        // Navigation property
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}