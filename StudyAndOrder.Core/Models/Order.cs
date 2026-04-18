using System;
using System.Collections.Generic;

namespace StudyAndOrder.Core.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;

        // FK
        public int StudyId { get; set; }
        public Study Study { get; set; } = null!;

        public DateTime CreationDate { get; set; }

        public ICollection<IngoingMaterial> IngoingMaterials { get; set; } = new List<IngoingMaterial>();

        // 1..1 produceret linje
        public OrderProducedMaterialLine ProducedMaterial { get; set; } = null!;
    }
}