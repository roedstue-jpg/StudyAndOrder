using System.Collections.Generic;

namespace StudyAndOrder.Core.Models
{
    public class Equipment
    {
        public int Id { get; set; }
        public string EquipmentId { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public ICollection<OrderProducedMaterialLine> ProducedMaterialLines { get; set; }
            = new List<OrderProducedMaterialLine>();
    }
}