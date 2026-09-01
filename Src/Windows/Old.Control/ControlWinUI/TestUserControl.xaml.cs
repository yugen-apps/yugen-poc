using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Windows.Input;

namespace ControlWinUI
{
    public sealed partial class TestUserControl : UserControl
    {
        public static readonly DependencyProperty NextCommandProperty = DependencyProperty.Register(
            nameof(NextCommand),
            typeof(ICommand),
            typeof(TestUserControl),
            new PropertyMetadata(default));

        public ICommand NextCommand
        {
            get => (ICommand)GetValue(NextCommandProperty);
            set => SetValue(NextCommandProperty, value);
        }

        public TestUserControl()
        {
            InitializeComponent();
        }
    }
}