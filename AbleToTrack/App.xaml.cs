using System.Windows;
using AbleToTrack.Services.Definitions;
using AbleToTrack.Services.Implementations;
using AbleToTrack.Services.Rest;
using AbleToTrack.ViewModels;
using AbleToTrack.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AbleToTrack
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public static IHost? AppHost { get; set; }

        public App()
        {
            /*<a href="https://www.flaticon.com/free-icons/stopwatch" title="stopwatch icons">Stopwatch icons created by Freepik - Flaticon</a>*/
            /*AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                var loggerFactory = host.Services.GetService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(args.ExceptionObject as Exception, "An error happened");
            };*/
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureLogging(logging => logging.AddConsole().AddDebug())
                .ConfigureServices((hostContext, services) =>
                {
                    CreateViews(services);

                    services.AddSingleton<UserManager>();
                    services.AddSingleton<ILoginService, LoginService>();
                }).Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();

            RequireViews();

            var loginView = AppHost.Services.GetRequiredService<LoginView>();
            loginView.DataContext = new LoginViewModel(AppHost.Services.GetRequiredService<ILoginService>());
            loginView.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();
            base.OnExit(e);
        }

        private void CreateViews(IServiceCollection services)
        {
            services.AddSingleton<AlertView>();
            services.AddSingleton<ForgotPasswordView>();
            services.AddSingleton<LoginView>();
        }

        private void RequireViews()
        {
            AppHost!.Services.GetRequiredService<AlertView>();
            AppHost.Services.GetRequiredService<ForgotPasswordView>().DataContext =
                new ForgotPasswordViewModel(AppHost.Services.GetRequiredService<ILoginService>());
        }
    }
}