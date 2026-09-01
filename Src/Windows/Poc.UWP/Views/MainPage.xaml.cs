using Microsoft.Extensions.DependencyInjection;
using Poc.UWP.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Poc.UWP.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            DataContext = App.Current.Services.GetService<MainViewModel>();
        }

        private MainViewModel ViewModel => (MainViewModel)DataContext;
    }
}