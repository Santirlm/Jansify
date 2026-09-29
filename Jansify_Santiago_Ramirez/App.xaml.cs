using Jansify_Santiago_Ramirez.Helpers;
using Jansify_Santiago_Ramirez.Views;
using System.Windows;

namespace Jansify_Santiago_Ramirez
{
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            await AuthService.SeedAdmin();
            var main = new MainWindow();
            main.Show();
        }
    }
}