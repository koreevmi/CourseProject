using ClosedXML.Excel;
using ConstructionMaterialsManager.Models;

namespace ConstructionMaterialsManager.Services
{
    public class ExcelService : IExcelService
    {
        public void GenerateMaterialsReport(IEnumerable<Material> materials, string filePath)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Материалы");

                // Заголовки столбцов
                worksheet.Cell(1, 1).Value = "Наименование";
                worksheet.Cell(1, 2).Value = "Ед. изм.";
                worksheet.Cell(1, 3).Value = "Стоимость";
                worksheet.Cell(1, 4).Value = "Поставщик";
                worksheet.Cell(1, 5).Value = "Остаток";

                // Данные
                int row = 2;
                foreach (var material in materials)
                {
                    worksheet.Cell(row, 1).Value = material.Name;
                    worksheet.Cell(row, 2).Value = material.Unit;
                    worksheet.Cell(row, 3).Value = material.CostPerUnit;
                    worksheet.Cell(row, 4).Value = material.Supplier?.Name ?? "Нет поставщика";
                    worksheet.Cell(row, 5).Value = material.CurrentStock;
                    row++;
                }

                workbook.SaveAs(filePath);
            }
        }

        public void GenerateProjectsReport(IEnumerable<Project> projects, string filePath)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Проекты");

                // Заголовки столбцов
                worksheet.Cell(1, 1).Value = "Название";
                worksheet.Cell(1, 2).Value = "Описание";
                worksheet.Cell(1, 3).Value = "Дата начала";
                worksheet.Cell(1, 4).Value = "Дата окончания";
                worksheet.Cell(1, 5).Value = "Бюджет";

                // Данные
                int row = 2;
                foreach (var project in projects)
                {
                    worksheet.Cell(row, 1).Value = project.Name;
                    worksheet.Cell(row, 2).Value = project.Description;
                    worksheet.Cell(row, 3).Value = project.StartDate.ToShortDateString();
                    worksheet.Cell(row, 4).Value = project.EndDate.ToShortDateString();
                    worksheet.Cell(row, 5).Value = project.Budget;
                    row++;
                }

                workbook.SaveAs(filePath);
            }
        }

        public void GenerateDeliveriesReport(IEnumerable<Delivery> deliveries, string filePath)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Поставки");

                // Заголовки столбцов
                worksheet.Cell(1, 1).Value = "Материал";
                worksheet.Cell(1, 2).Value = "Количество";
                worksheet.Cell(1, 3).Value = "Дата поставки";
                worksheet.Cell(1, 4).Value = "Поставщик";
                worksheet.Cell(1, 5).Value = "Стоимость";

                // Данные
                int row = 2;
                foreach (var delivery in deliveries)
                {
                    worksheet.Cell(row, 1).Value = delivery.Material?.Name ?? "Неизвестно";
                    worksheet.Cell(row, 2).Value = delivery.Quantity;
                    worksheet.Cell(row, 3).Value = delivery.DeliveryDate.ToShortDateString();
                    worksheet.Cell(row, 4).Value = delivery.Supplier?.Name ?? "Неизвестно";
                    worksheet.Cell(row, 5).Value = delivery.Quantity * delivery.Material?.CostPerUnit ?? 0;
                    row++;
                }

                workbook.SaveAs(filePath);
            }
        }

        public void GenerateMaterialMovementsReport(IEnumerable<MaterialMovement> movements, string filePath)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Движения");

                // Заголовки столбцов
                worksheet.Cell(1, 1).Value = "Материал";
                worksheet.Cell(1, 2).Value = "Количество";
                worksheet.Cell(1, 3).Value = "Дата движения";
                worksheet.Cell(1, 4).Value = "Тип движения";
                worksheet.Cell(1, 5).Value = "Остаток";

                // Данные
                int row = 2;
                foreach (var movement in movements)
                {
                    worksheet.Cell(row, 1).Value = movement.Material?.Name ?? "Неизвестно";
                    worksheet.Cell(row, 2).Value = movement.Quantity;
                    worksheet.Cell(row, 3).Value = movement.MovementDate.ToString("dd.MM.yyyy");
                    worksheet.Cell(row, 4).Value = movement.MovementType;
                    worksheet.Cell(row, 5).Value = movement.Material?.CurrentStock ?? 0;
                    row++;
                }

                workbook.SaveAs(filePath);
            }
        }
    }
}
