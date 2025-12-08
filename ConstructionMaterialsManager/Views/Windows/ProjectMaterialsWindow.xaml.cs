using ConstructionMaterialsManager.Data;
using ConstructionMaterialsManager.Models;
using ConstructionMaterialsManager.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace ConstructionMaterialsManager.Views.Windows
{
    public partial class ProjectMaterialsWindow : Window
    {
        private readonly IDatabaseService _databaseService;
        private readonly IServiceProvider _serviceProvider;
        private Project _project;
        private int _projectId;

        public ProjectMaterialsWindow(IDatabaseService databaseService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }


        public void SetProject(int projectId)
        {
            if (projectId <= 0)
            {
                throw new ArgumentException("ProjectId должен быть больше нуля.", nameof(projectId));
            }

            _projectId = projectId;
            LoadProjectMaterials();
        }

        private void LoadProjectMaterials()
        {
            try
            {
                var projectMaterials = _databaseService.GetProjectMaterials(_projectId);
                ProjectMaterialsDataGrid.ItemsSource = projectMaterials;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке материалов проекта: {ex.Message}");
            }
        }


        private void AddMaterialButton_Click(object sender, RoutedEventArgs e)
        {
            if (_projectId <= 0)
            {
                MessageBox.Show("Проект не выбран.");
                return;
            }

            try
            {
                var materialSelectionWindow = _serviceProvider.GetRequiredService<MaterialSelectionWindow>();
                materialSelectionWindow.SetProject(_projectId);
                if (materialSelectionWindow.ShowDialog() == true)
                {
                    LoadProjectMaterials();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении материала: {ex.Message}");
            }
        }

        private void RemoveMaterialButton_Click(object sender, RoutedEventArgs e)
        {
            try
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
            catch (DbUpdateException dbEx)
            {
                MessageBox.Show($"Ошибка при удалении материала: {dbEx.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении материала: {ex.Message}");
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
