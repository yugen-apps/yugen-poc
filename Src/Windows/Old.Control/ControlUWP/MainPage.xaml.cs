using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ControlUWP
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            NextCommand = new RelayCommand(Next);
        }

        public IRelayCommand NextCommand { get; }

        public void Next()
        {
        }
    }

    public sealed class CustomControl1 : Control
    {
        public static readonly DependencyProperty NextCommandProperty = DependencyProperty.Register(
            nameof(NextCommand),
            typeof(ICommand),
            typeof(CustomControl1),
            new PropertyMetadata(default));

        public ICommand NextCommand
        {
            get => (ICommand)GetValue(NextCommandProperty);
            set => SetValue(NextCommandProperty, value);
        }

        public CustomControl1()
        {
            this.DefaultStyleKey = typeof(CustomControl1);
        }
    }
}
