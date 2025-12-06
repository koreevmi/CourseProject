using ConstructionMaterialsManager.Models;

namespace ConstructionMaterialsManager.Services
{
    public interface IExcelService
    {
        void GenerateMaterialsReport(IEnumerable<Material> materials, string filePath);
        void GenerateProjectsReport(IEnumerable<Project> projects, string filePath);
        void GenerateDeliveriesReport(IEnumerable<Delivery> deliveries, string filePath);
        void GenerateMaterialMovementsReport(IEnumerable<MaterialMovement> movements, string filePath);
    }
}
