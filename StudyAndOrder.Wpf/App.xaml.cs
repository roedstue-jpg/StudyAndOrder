using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StudyAndOrder.Core.Data;
using StudyAndOrder.Wpf.Repositories;
using StudyAndOrder.Wpf.ViewModels;
using System.Windows;

namespace StudyAndOrder.Wpf
{
    public partial class App : Application
    {
        private readonly IHost _host;
        private IServiceScope? _scope;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<AppDbContext>(options =>
                        options.UseSqlServer(
                            "Server=(localdb)\\mssqllocaldb;Database=SOMS_Db;Trusted_Connection=True;"));

                    services.AddScoped<IMaterialRepository, MaterialRepository>();
                    services.AddScoped<IEquipmentRepository, EquipmentRepository>();

                    // ViewModels (scoped er ok)
                    services.AddScoped<OrdersViewModel>();
                    services.AddScoped<StudyInformationViewModel>();
                    services.AddScoped<OrderDetailsViewModel>();
                    services.AddScoped<MainViewModel>();

                    // MainWindow
                    services.AddScoped<MainWindow>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            // Migrationer: brug en midlertidig scope
            using (var migrationScope = _host.Services.CreateScope())
            {
                var db = migrationScope.ServiceProvider.GetRequiredService<AppDbContext>();
                await db.Database.MigrateAsync();
            }

            // UI: opret en ny scope der lever indtil OnExit
            _scope = _host.Services.CreateScope();
            var mainWindow = _scope.ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _scope?.Dispose();
            _host.Dispose();
            base.OnExit(e);
        }
    }
}