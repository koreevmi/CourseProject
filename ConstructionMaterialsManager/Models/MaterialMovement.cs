namespace ConstructionMaterialsManager.Models;

public partial class MaterialMovement
{
    public int MovementId { get; set; }

    public int MaterialId { get; set; }

    public decimal Quantity { get; set; }

    public string MovementType { get; set; } = null!;

    public DateTime MovementDate { get; set; }

    public virtual Material Material { get; set; } = null!;
}
