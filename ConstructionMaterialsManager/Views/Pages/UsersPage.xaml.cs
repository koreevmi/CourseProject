using ConstructionMaterialsManager.Models;
using ConstructionMaterialsManager.Services;
using ConstructionMaterialsManager.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace ConstructionMaterialsManager.Views.Pages
{
    public partial class UsersPage : UserControl
    {
        private readonly IDatabaseService _databaseService;
        private readonly IServiceProvider _serviceProvider;
        private ObservableCollection<User> _users;

        public UsersPage(IDatabaseService databaseService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _databaseService = databaseService;
            _serviceProvider = serviceProvider;

            // Инициализируем коллекцию
            _users = new ObservableCollection<User>();
            DataGrid.ItemsSource = _users;

            LoadUsers();
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (App.CurrentUser?.Role != "Администратор")
            {
                AddBtn.Visibility = Visibility.Collapsed;
                EditBtn.Visibility = Visibility.Collapsed;
                DeleteBtn.Visibility = Visibility.Collapsed;
            }
        }

        private void LoadUsers()
        {
            _users.Clear();
            foreach (var user in _databaseService.GetUsers())
            {
                _users.Add(user);
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var userWindow = _serviceProvider.GetRequiredService<UserWindow>();
            if (userWindow.ShowDialog() == true)
            {
                LoadUsers();
            }
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = DataGrid.SelectedItem as User;
            if (selectedUser != null)
            {
                var userWindow = _serviceProvider.GetRequiredService<UserWindow>();
                userWindow.SetUser(selectedUser);
                if (userWindow.ShowDialog() == true)
                {
                    LoadUsers();
                }
            }
            else
            {
                MessageBox.Show("Выберите пользователя для редактирования.");
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (App.CurrentUser?.Role != "Администратор")
            {
                MessageBox.Show("Только администратор может удалять пользователей.");
                return;
            }

            var selectedUser = DataGrid.SelectedItem as User;
            if (selectedUser != null)
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить этого пользователя?",
                    "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    _databaseService.DeleteUser(selectedUser.UserId);
                    LoadUsers();
                }
            }
            else
            {
                MessageBox.Show("Выберите пользователя для удаления.");
            }
        }
    }
}
