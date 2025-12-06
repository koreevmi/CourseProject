using ConstructionMaterialsManager.Services;
using Microsoft.Extensions.Logging;
using System.Windows;
using System.Windows.Controls;

namespace ConstructionMaterialsManager.Views.Pages
{
    public partial class ReportsPage : UserControl
    {
        private readonly IDatabaseService _databaseService;
        private readonly IExcelService _excelService;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ReportsPage> _logger;

        public ReportsPage(
            IDatabaseService databaseService,
            IExcelService excelService,
            IServiceProvider serviceProvider,
            ILogger<ReportsPage> logger)
        {
            InitializeComponent();
            _databaseService = databaseService;
            _excelService = excelService;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        private void GenerateExcelReport<T>(
            Func<IEnumerable<T>> getData,
            Action<IEnumerable<T>, string> generateReport,
            string defaultFileName,
            string reportType)
        {
            try
            {
                ReportProgressBar.Visibility = Visibility.Visible;
                ReportStatusLabel.Text = $"Генерация Excel-отчёта по {reportType}...";

                var data = getData();
                if (data == null || !data.Any())
                {
                    ReportStatusLabel.Text = $"Нет данных для генерации отчёта по {reportType}.";
                    return;
                }

                var saveFileDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "Excel files (*.xlsx)|*.xlsx",
                    FileName = defaultFileName,
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    generateReport(data, saveFileDialog.FileName);
                    ReportStatusLabel.Text = $"Excel-отчет по {reportType} успешно сохранен: {saveFileDialog.FileName}";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при генерации Excel-отчёта по {reportType}");
                MessageBox.Show(
                    $"Ошибка при генерации Excel-отчета по {reportType}: {ex.Message}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
            finally
            {
                ReportProgressBar.Visibility = Visibility.Collapsed;
            }
        }

        private void MaterialsExcelReportButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateExcelReport(
                () => _databaseService.GetMaterials(),
                (materials, filePath) => _excelService.GenerateMaterialsReport(materials, filePath),
                $"MaterialsReport_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
                "материалам"
            );
        }

        private void ProjectsExcelReportButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateExcelReport(
                () => _databaseService.GetProjects(),
                (projects, filePath) => _excelService.GenerateProjectsReport(projects, filePath),
                $"ProjectsReport_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
                "проектам"
            );
        }

        private void DeliveriesExcelReportButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateExcelReport(
                () => _databaseService.GetDeliveries(),
                (deliveries, filePath) => _excelService.GenerateDeliveriesReport(deliveries, filePath),
                $"DeliveriesReport_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
                "поставкам"
            );
        }

        private void MaterialMovementsExcelReportButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateExcelReport(
                () => _databaseService.GetMaterialMovements(),
                (movements, filePath) => _excelService.GenerateMaterialMovementsReport(movements, filePath),
                $"MaterialMovementsReport_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
                "движениям материалов"
            );
        }
    }
}
