using System.Collections.Generic;

namespace StudyAndOrder.Core.Models
{
    // Den "producerede" linje på en Order
    public class OrderProducedMaterialLine
    {
        public int Id { get; set; }

        // Foreign key til Order
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        // Produceret materiale (nummer kommer fra dropdown/DB)
        public string MaterialNumber { get; set; } = string.Empty;

        // Forventet output
        public string ExpectedOutcome { get; set; } = string.Empty;

        // Multi-select equipment knyttet til det producerede materiale
        public ICollection<Equipment> Equipments { get; set; } = new List<Equipment>();

    }
}