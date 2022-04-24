using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RecordsTable.Models;
using RecordsTable.Services;
using RecordsTable.Services.API;
using RecordsTable.View;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows;

namespace RecordsTable
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, configurationBuilder) =>
                {
                    configurationBuilder.SetBasePath(context.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", optional: false);
                })
                .ConfigureServices((context, services) =>
                {
                    services.ConfigureWritable<Settings>(context.Configuration.GetSection("Settings"))
                        .AddSingleton<MainWindow>()
                        .AddSingleton<HttpClient>()
                        .AddSingleton<AuthService>()
                        .AddSingleton<ScheduleGenerationService>()
                        .AddSingleton<RestAPIService>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();
            AuthService authService = _host.Services.GetRequiredService<AuthService>();
            authService.Auth("/auth");
            RestAPIService restAPI = _host.Services.GetRequiredService<RestAPIService>();
            //Тестовый запрос (Рабочий)
            //List<Seanses> list = await restAPI.Get<List<Seanses>>("/timetable/seances/691974/1992989/2022-06-01", null);
            MainWindow mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync();
            }

            base.OnExit(e);
        }
    }
}
