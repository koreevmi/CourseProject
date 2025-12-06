namespace ConstructionMaterialsManager.Models;

public partial class Delivery
{
    public int DeliveryId { get; set; }

    public int MaterialId { get; set; }

    public decimal Quantity { get; set; }

    public DateTime DeliveryDate { get; set; }

    public int SupplierId { get; set; }

    public virtual Material Material { get; set; } = null!;

    public virtual Supplier Supplier { get; set; } = null!;
}
