using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using SparkLauncher.WinUI.UIComponent.Utils;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SparkLauncher.WinUI {
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application {
        public static IServiceProvider Services {
            get {
                IServiceProvider serviceProvider = ((App)Current)._serviceProvider;
                return serviceProvider ?? throw new InvalidOperationException("The service provider is not initialized");
            }
        }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App() {
            this.RequestedTheme = ApplicationTheme.Light;
            this.InitializeComponent();

            _serviceProvider = ConfigureServices();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args) {
            // ref: https://github.com/AndrewKeepCoding/WinUI3Localizer
            await LanguageUtil.InitializeLocalizerForUnpackaged(args.Arguments.Length == 0 ? "zh-CN" : args.Arguments);

            m_window = App.Services.GetRequiredService<MainWindow>();
            m_window.Activate();
        }

        private static ServiceProvider ConfigureServices() {
            var provider = new ServiceCollection()
                .AddSingleton<MainWindow>()

                .AddHttpClient()

                .BuildServiceProvider();

            return provider;
        }

        private Window m_window;
        private readonly IServiceProvider _serviceProvider;
    }
}
