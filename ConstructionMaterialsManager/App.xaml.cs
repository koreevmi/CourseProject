using ConstructionMaterialsManager.Data;
using ConstructionMaterialsManager.Models;
using ConstructionMaterialsManager.Services;
using ConstructionMaterialsManager.Views.Pages;
using ConstructionMaterialsManager.Views.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Windows;

namespace ConstructionMaterialsManager
{
    public partial class App : Application
    {
        public static User CurrentUser { get; set; }

        public IServiceProvider ServiceProvider { get; private set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();

            var loginWindow = ServiceProvider.GetRequiredService<LoginWindow>();
            loginWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer("Server=DESKTOP-JTTSOIJ;Database=RoadConstructionDB;User Id=koreev;Password=1234;TrustServerCertificate=True;"));

            services.AddScoped<IDatabaseService, DatabaseService>();

            services.AddSingleton<IExcelService, ExcelService>();
            services.AddLogging(configure => configure.AddDebug().AddConsole());

            services.AddTransient<LoginWindow>();
            services.AddTransient<MainWindow>();
            services.AddTransient<MaterialsPage>();
            services.AddTransient<SuppliersPage>();
            services.AddTransient<ProjectsPage>();
            services.AddTransient<DeliveriesPage>();
            services.AddTransient<ReportsPage>();
            services.AddTransient<UsersPage>();
            services.AddTransient<MaterialWindow>();
            services.AddTransient<SupplierWindow>();
            services.AddTransient<ProjectWindow>();
            services.AddTransient<DeliveryWindow>();
            services.AddTransient<UserWindow>();
            services.AddTransient<ProjectMaterialsWindow>();
            services.AddTransient<MaterialSelectionWindow>();

            ServiceProvider = services.BuildServiceProvider();
        }

    }
}
