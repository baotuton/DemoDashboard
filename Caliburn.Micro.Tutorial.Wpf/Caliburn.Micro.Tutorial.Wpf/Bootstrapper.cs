using Caliburn.Micro.Tutorial.Wpf.ViewModels;
using System.Windows;

namespace Caliburn.Micro.Tutorial.Wpf
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }

        protected override async void OnStartup(object sender, StartupEventArgs e)
        {
            await DisplayRootViewForAsync(typeof(ShellViewModel));
        }
    }
}
