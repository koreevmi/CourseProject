namespace ConstructionMaterialsManager.Models;

public partial class ProjectMaterial
{
    public int ProjectMaterialId { get; set; }

    public int ProjectId { get; set; }

    public int MaterialId { get; set; }

    public decimal PlannedQuantity { get; set; }

    public decimal? UsedQuantity { get; set; }

    public virtual Material Material { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;
}
