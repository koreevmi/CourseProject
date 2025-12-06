using ConstructionMaterialsManager.Models;
using ConstructionMaterialsManager.Services;
using System.Windows;

namespace ConstructionMaterialsManager.Views.Windows
{
    public partial class MaterialWindow : Window
    {
        private readonly IDatabaseService _databaseService;
        private readonly IServiceProvider _serviceProvider;
        private Material _material;
        private bool _isEditMode;

        public MaterialWindow(IDatabaseService databaseService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _databaseService = databaseService;
            _serviceProvider = serviceProvider;

            // Загружаем поставщиков
            SupplierComboBox.ItemsSource = _databaseService.GetSuppliers();
        }

        public void SetMaterial(Material material)
        {
            _material = material;
            _isEditMode = true;

            NameTextBox.Text = material.Name;
            UnitTextBox.Text = material.Unit;
            CostPerUnitTextBox.Text = material.CostPerUnit.ToString();
            SupplierComboBox.SelectedValue = material.SupplierId;
            CurrentStockTextBox.Text = material.CurrentStock.ToString();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NameTextBox.Text) ||
                string.IsNullOrEmpty(UnitTextBox.Text) ||
                !decimal.TryParse(CostPerUnitTextBox.Text, out decimal costPerUnit) ||
                SupplierComboBox.SelectedValue == null ||
                !decimal.TryParse(CurrentStockTextBox.Text, out decimal currentStock))
            {
                MessageBox.Show("Пожалуйста, заполните все поля корректно.");
                return;
            }

            if (_isEditMode)
            {
                _material.Name = NameTextBox.Text;
                _material.Unit = UnitTextBox.Text;
                _material.CostPerUnit = costPerUnit;
                _material.SupplierId = (int)SupplierComboBox.SelectedValue;
                _material.CurrentStock = currentStock;
                _databaseService.UpdateMaterial(_material);
            }
            else
            {
                var material = new Material
                {
                    Name = NameTextBox.Text,
                    Unit = UnitTextBox.Text,
                    CostPerUnit = costPerUnit,
                    SupplierId = (int)SupplierComboBox.SelectedValue,
                    CurrentStock = currentStock
                };
                _databaseService.AddMaterial(material);
            }

            DialogResult = true;
            Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
