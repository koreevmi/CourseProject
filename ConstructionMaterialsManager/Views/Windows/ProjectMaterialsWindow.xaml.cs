using ConstructionMaterialsManager.Models;
using ConstructionMaterialsManager.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace ConstructionMaterialsManager.Views.Windows
{
    public partial class ProjectMaterialsWindow : Window
    {
        private readonly IDatabaseService _databaseService;
        private readonly IServiceProvider _serviceProvider;
        private Project _project;

        public ProjectMaterialsWindow(IDatabaseService databaseService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _databaseService = databaseService;
            _serviceProvider = serviceProvider;
        }

        public void SetProject(Project project)
        {
            _project = project;
            LoadProjectMaterials();
        }

        private void LoadProjectMaterials()
        {
            var projectMaterials = _databaseService.GetProjectMaterials(_project.ProjectId);
            ProjectMaterialsDataGrid.ItemsSource = projectMaterials;
        }

        private void AddMaterialButton_Click(object sender, RoutedEventArgs e)
        {
            var materialSelectionWindow = _serviceProvider.GetRequiredService<MaterialSelectionWindow>();
            materialSelectionWindow.SetProject(_project.ProjectId);
            if (materialSelectionWindow.ShowDialog() == true)
            {
                LoadProjectMaterials();
            }
        }

        private void RemoveMaterialButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedProjectMaterial = ProjectMaterialsDataGrid.SelectedItem as ProjectMaterial;
            if (selectedProjectMaterial != null)
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить этот материал из проекта?",
                    "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    _databaseService.RemoveProjectMaterial(selectedProjectMaterial.ProjectMaterialId);
                    LoadProjectMaterials();
                }
            }
            else
            {
                MessageBox.Show("Выберите материал для удаления.");
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
