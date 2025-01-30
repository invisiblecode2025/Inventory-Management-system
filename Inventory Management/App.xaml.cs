using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using Microsoft.Extensions.Hosting;
using System.Data;
using System.Windows;
using Inventory_Management.ViewModels;
using Inventory.Services.Interface;
using Inventory.Services.Services;
using Inventory.Infrastructure.Interface;
using Inventory.Infrastructure.Repository.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Inventory.Repository.DataContext;
using AutoMapper;
using InventoryAPI.Mapper;

using Inventory_Management;
using Inventory_Management.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Windows.Input;

namespace Inventory_Management
{
    public partial class App : Application
    {

        private readonly IHost host;
        public static IServiceProvider ServiceProvider { get; set; }
        public App()
        {
            host = Host.CreateDefaultBuilder()
                     .ConfigureServices((context, services) => { ConfigureServices(context.Configuration, services); })
                     .ConfigureLogging(logBuilder =>
                     {
                         logBuilder.ClearProviders();
                         logBuilder.SetMinimumLevel(LogLevel.Information);
                     })
                     .Build();
            ServiceProvider = host.Services;

        }

        private void ConfigureServices(IConfiguration configuration,
        IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));

            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            services.AddTransient<MainWindow>();
            services.AddTransient<ItemViewModel>();
            services.AddTransient<CategoryViewModel>();
            services.AddTransient<InventoryViewModel>();
            services.AddTransient<SupplierViewModel>();
            services.AddTransient<MainWindow>();

            services.AddSingleton<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IItemServices, ItemServices>();
            services.AddScoped<ICategoryServices, CategoryServices>();
            services.AddScoped<ISupplierServices, SupplierServices>();
            services.AddScoped<IInventoryServices, InventoryServices>();

  

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Scoped);


            Mouse.OverrideCursor = Cursors.Wait;
        }
 

        protected override async void OnStartup(StartupEventArgs e)
        {
            await host.StartAsync();
            var mainWindow = host.Services.GetRequiredService<MainWindow>();
            mainWindow.WindowState = WindowState.Maximized;
            mainWindow.Show();
            base.OnStartup(e);

            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            // Handle global exceptions
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            DispatcherUnhandledException += App_DispatcherUnhandledException;


        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Error(e.ExceptionObject as Exception, "An unhandled exception occurred.");
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Error(e.Exception, "An unhandled dispatcher exception occurred.");
            e.Handled = true;
        }
        protected override async void OnExit(ExitEventArgs e)
        {
            await host.StopAsync();
            Log.CloseAndFlush();
            host.Dispose();
            base.OnExit(e);
        }
    }
}


