using ConstructionMaterialsManager.Models;
using ConstructionMaterialsManager.Services;
using System.Windows;

namespace ConstructionMaterialsManager.Views.Windows
{
    public partial class DeliveryWindow : Window
    {
        private readonly IDatabaseService _databaseService;
        private Delivery _delivery;
        private bool _isEditMode;

        public DeliveryWindow(IDatabaseService databaseService)
        {
            InitializeComponent();
            _databaseService = databaseService;
            LoadMaterials();
            LoadSuppliers();
        }

        private void LoadMaterials()
        {
            MaterialComboBox.ItemsSource = _databaseService.GetMaterials();
        }

        private void LoadSuppliers()
        {
            SupplierComboBox.ItemsSource = _databaseService.GetSuppliers();
        }

        public void SetDelivery(Delivery delivery)
        {
            _delivery = delivery;
            _isEditMode = true;

            MaterialComboBox.SelectedValue = delivery.MaterialId;
            QuantityTextBox.Text = delivery.Quantity.ToString();
            DeliveryDatePicker.SelectedDate = delivery.DeliveryDate;
            SupplierComboBox.SelectedValue = delivery.SupplierId;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (MaterialComboBox.SelectedValue == null || !decimal.TryParse(QuantityTextBox.Text, out decimal quantity) ||
                DeliveryDatePicker.SelectedDate == null || SupplierComboBox.SelectedValue == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля корректно.");
                return;
            }

            if (_isEditMode)
            {
                _delivery.MaterialId = (int)MaterialComboBox.SelectedValue;
                _delivery.Quantity = quantity;
                _delivery.DeliveryDate = (DateTime)DeliveryDatePicker.SelectedDate;
                _delivery.SupplierId = (int)SupplierComboBox.SelectedValue;
                _databaseService.UpdateDelivery(_delivery);
            }
            else
            {
                var delivery = new Delivery
                {
                    MaterialId = (int)MaterialComboBox.SelectedValue,
                    Quantity = quantity,
                    DeliveryDate = (DateTime)DeliveryDatePicker.SelectedDate,
                    SupplierId = (int)SupplierComboBox.SelectedValue
                };
                _databaseService.AddDelivery(delivery);

                var movement = new MaterialMovement
                {
                    MaterialId = (int)MaterialComboBox.SelectedValue,
                    Quantity = quantity,
                    MovementDate = DateTime.Now,
                    MovementType = "Поступление"
                };

                _databaseService.AddMaterialMovement(movement);
            }

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
