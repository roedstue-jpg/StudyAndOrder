using System.Collections.Generic;

namespace StudyAndOrder.Core.Models
{
    public class OrderProducedMaterialLine
    {
        public int Id { get; set; }

        


        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

      
        public int MaterialId { get; set; }
        public Material Material { get; set; } = null!;

        

        public string ExpectedOutcome { get; set; } = string.Empty;
        public ICollection<Equipment> Equipments { get; set; }
            = new List<Equipment>();
    }
}