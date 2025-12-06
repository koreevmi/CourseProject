using ConstructionMaterialsManager.Views.Pages;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace ConstructionMaterialsManager.Views.Windows
{
    public partial class MainWindow : Window
    {
        private readonly IServiceProvider _serviceProvider;

        public MainWindow(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (App.CurrentUser != null)
            {
                UserInfoLabel.Content = $"Пользователь: {App.CurrentUser.FullName} ({App.CurrentUser.Role})";

                if (App.CurrentUser.Role == "Администратор")
                {
                    UsersMenuItem.Visibility = Visibility.Visible;
                }
            }
        }

        private void MaterialsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var materialsPage = _serviceProvider.GetRequiredService<MaterialsPage>();
            MainFrame.Content = materialsPage;
        }

        private void SuppliersMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var suppliersPage = _serviceProvider.GetRequiredService<SuppliersPage>();
            MainFrame.Content = suppliersPage;
        }

        private void ProjectsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var projectsPage = _serviceProvider.GetRequiredService<ProjectsPage>();
            MainFrame.Content = projectsPage;
        }

        private void DeliveriesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var deliveriesPage = _serviceProvider.GetRequiredService<DeliveriesPage>();
            MainFrame.Content = deliveriesPage;
        }

        private void ReportsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var reportsPage = _serviceProvider.GetRequiredService<ReportsPage>();
            MainFrame.Content = reportsPage;
        }

        private void UsersMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var usersPage = _serviceProvider.GetRequiredService<UsersPage>();
            MainFrame.Content = usersPage;
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            App.CurrentUser = null;
            var loginWindow = _serviceProvider.GetRequiredService<LoginWindow>();
            loginWindow.Show();
            Close();
        }
    }
}
