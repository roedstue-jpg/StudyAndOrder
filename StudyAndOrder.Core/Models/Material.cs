using StudyAndOrder.Core.Models;

public class Material
{
    public int Id { get; set; }
    public string MaterialNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Navigation property
    // Et materiale kan bruges i mange ordrer
    public ICollection<OrderProducedMaterialLine> ProducedMaterialLines { get; set; }
        = new List<OrderProducedMaterialLine>();
}