using System.Windows;
using AbleToTrack.Events.Dialogs;
using AbleToTrack.Services.Definitions;
using AbleToTrack.Services.Implementations;
using AbleToTrack.ViewModels;
using AbleToTrack.Views;
using CommunityToolkit.Mvvm.Messaging;
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
            /*AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                var loggerFactory = host.Services.GetService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(args.ExceptionObject as Exception, "An error happened");
            };*/
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureLogging(logging => logging.AddConsole().AddDebug())
                .ConfigureServices((_, services) =>
                {
                    CreateViews(services);

                    services.AddSingleton<IUserManager, UserManager>();
                    services.AddSingleton<ILoginService, LoginService>();
                }).Build();
            
            WeakReferenceMessenger.Default.Register<CloseCurrentWindowRequested>(this, (_, _) => OnCloseCurrentWindowRequested());
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
            services.AddSingleton<MainView>();
        }

        private void RequireViews()
        {
            AppHost!.Services.GetRequiredService<AlertView>();
            AppHost.Services.GetRequiredService<ForgotPasswordView>().DataContext =
                new ForgotPasswordViewModel(AppHost.Services.GetRequiredService<ILoginService>());
            AppHost.Services.GetRequiredService<MainView>().DataContext =
                new MainViewModel();
        }

        private void OnCloseCurrentWindowRequested()
        {
            Current.Windows[1]?.Hide();
        }
    }
}