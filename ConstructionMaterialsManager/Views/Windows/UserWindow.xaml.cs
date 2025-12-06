using ConstructionMaterialsManager.Models;
using ConstructionMaterialsManager.Services;
using System.Windows;
using System.Windows.Controls;

namespace ConstructionMaterialsManager.Views.Windows
{
    public partial class UserWindow : Window
    {
        private readonly IDatabaseService _databaseService;
        private readonly IServiceProvider _serviceProvider;
        private User _user;
        private bool _isEditMode;

        public UserWindow(IDatabaseService databaseService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _databaseService = databaseService;
            _serviceProvider = serviceProvider;
        }

        public void SetUser(User user)
        {
            _user = user;
            _isEditMode = true;

            LoginTextBox.Text = user.Login;
            FullNameTextBox.Text = user.FullName;
            EmailTextBox.Text = user.Email;
            RoleComboBox.Text = user.Role;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(LoginTextBox.Text) ||
                string.IsNullOrEmpty(FullNameTextBox.Text) ||
                string.IsNullOrEmpty(PasswordBox.Password) ||
                RoleComboBox.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.");
                return;
            }

            if (_isEditMode)
            {
                _user.Login = LoginTextBox.Text;
                _user.FullName = FullNameTextBox.Text;
                if (!string.IsNullOrEmpty(PasswordBox.Password))
                {
                    _user.Password = PasswordBox.Password;
                }
                _user.Email = EmailTextBox.Text;
                _user.Role = ((ComboBoxItem)RoleComboBox.SelectedItem).Content.ToString();
                _databaseService.UpdateUser(_user);
            }
            else
            {
                var user = new User
                {
                    Login = LoginTextBox.Text,
                    Password = PasswordBox.Password,
                    FullName = FullNameTextBox.Text,
                    Email = EmailTextBox.Text,
                    Role = ((ComboBoxItem)RoleComboBox.SelectedItem).Content.ToString()
                };
                _databaseService.AddUser(user);
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
