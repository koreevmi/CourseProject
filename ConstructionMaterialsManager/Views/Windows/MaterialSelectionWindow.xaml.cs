using ConstructionMaterialsManager.Models;
using ConstructionMaterialsManager.Services;
using System.Windows;

namespace ConstructionMaterialsManager.Views.Windows
{
    public partial class MaterialSelectionWindow : Window
    {
        private readonly IDatabaseService _databaseService;
        private int _projectId;

        public MaterialSelectionWindow(IDatabaseService databaseService)
        {
            InitializeComponent();
            _databaseService = databaseService;
        }

        public void SetProject(int projectId)
        {
            _projectId = projectId;
            LoadMaterials();
        }

        private void LoadMaterials()
        {
            MaterialsDataGrid.ItemsSource = _databaseService.GetMaterials();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedMaterial = MaterialsDataGrid.SelectedItem as Material;
            if (selectedMaterial != null && decimal.TryParse(QuantityTextBox.Text, out decimal quantity) && quantity > 0)
            {
                var projectMaterial = new ProjectMaterial
                {
                    ProjectId = _projectId,
                    MaterialId = selectedMaterial.MaterialId,
                    PlannedQuantity = quantity,
                    UsedQuantity = 0
                };

                _databaseService.AddProjectMaterial(projectMaterial);
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Выберите материал и укажите корректное количество.");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
