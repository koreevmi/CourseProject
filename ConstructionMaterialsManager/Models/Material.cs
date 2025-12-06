namespace ConstructionMaterialsManager.Models;

public partial class Material
{
    public int MaterialId { get; set; }

    public string Name { get; set; } = null!;

    public string Unit { get; set; } = null!;

    public decimal CostPerUnit { get; set; }

    public int SupplierId { get; set; }

    public decimal? CurrentStock { get; set; }

    public virtual ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();

    public virtual ICollection<MaterialMovement> MaterialMovements { get; set; } = new List<MaterialMovement>();

    public virtual ICollection<ProjectMaterial> ProjectMaterials { get; set; } = new List<ProjectMaterial>();

    public virtual Supplier Supplier { get; set; } = null!;
}
