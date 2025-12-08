using ConstructionMaterialsManager.Models;
using ConstructionMaterialsManager.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace ConstructionMaterialsManager.Views.Windows
{
    public partial class ProjectWindow : Window
    {
        private readonly IDatabaseService _databaseService;
        private readonly IServiceProvider _serviceProvider;
        private Project _project;
        private bool _isEditMode;


        public ProjectWindow(IDatabaseService databaseService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _databaseService = databaseService;
            _serviceProvider = serviceProvider;
            _project = new Project(); // Инициализация нового проекта
        }


        public void SetProject(Project project)
        {
            _project = project;
            _isEditMode = true;

            ProjectNameTextBox.Text = project.Name;
            ProjectDescriptionTextBox.Text = project.Description;
            ProjectStartDatePicker.SelectedDate = project.StartDate;
            ProjectEndDatePicker.SelectedDate = project.EndDate;
            ProjectBudgetTextBox.Text = project.Budget.ToString();
            ProjectStatusComboBox.Text = project.Status;

        }
        
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ProjectNameTextBox.Text) ||
                string.IsNullOrEmpty(ProjectDescriptionTextBox.Text) ||
                ProjectStartDatePicker.SelectedDate == null ||
                ProjectEndDatePicker.SelectedDate == null ||
                !decimal.TryParse(ProjectBudgetTextBox.Text, out decimal budget) ||
                ProjectStatusComboBox.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля корректно.");
                return;
            }

            try
            {
                if (_isEditMode)
                {
                    _project.Name = ProjectNameTextBox.Text;
                    _project.Description = ProjectDescriptionTextBox.Text;
                    _project.StartDate = (DateTime)ProjectStartDatePicker.SelectedDate;
                    _project.EndDate = (DateTime)ProjectEndDatePicker.SelectedDate;
                    _project.Budget = budget;
                    _project.Status = ((ComboBoxItem)ProjectStatusComboBox.SelectedItem).Content.ToString();
                    _databaseService.UpdateProject(_project);
                }
                else
                {
                    _project = new Project
                    {
                        Name = ProjectNameTextBox.Text,
                        Description = ProjectDescriptionTextBox.Text,
                        StartDate = (DateTime)ProjectStartDatePicker.SelectedDate,
                        EndDate = (DateTime)ProjectEndDatePicker.SelectedDate,
                        Budget = budget,
                        Status = ((ComboBoxItem)ProjectStatusComboBox.SelectedItem).Content.ToString()
                    };
                    _databaseService.AddProject(_project);
                }
                DialogResult = true;
                Close();
            }
            catch (DbUpdateException dbEx)
            {
                MessageBox.Show($"Ошибка при сохранении проекта: {dbEx.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении проекта: {ex.Message}");
            }
        }



        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
