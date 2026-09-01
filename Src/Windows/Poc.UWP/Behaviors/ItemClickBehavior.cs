using Microsoft.Xaml.Interactivity;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Poc.UWP.Behaviors
{
    public sealed class ItemClickBehavior : Behavior<ListViewBase>
    {
        #region DependencyProperties

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            nameof(Command),
            typeof(ICommand),
            typeof(ItemClickBehavior),
            new PropertyMetadata(default(ICommand)));

        public ICommand Command
        {
            get => (ICommand)this.GetValue(CommandProperty);
            set => this.SetValue(CommandProperty, value);
        }

        public string Id { get; set; }

        #endregion DependencyProperties

        protected override void OnAttached()
        {
            base.OnAttached();

            if (this.AssociatedObject != null)
            {
                this.AssociatedObject.ItemClick += this.HandleItemClick;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (this.AssociatedObject != null)
            {
                this.AssociatedObject.ItemClick -= this.HandleItemClick;
            }
        }

        private void HandleItemClick(object sender, ItemClickEventArgs e)
        {
            //if (!(this.Command is ICommand command) ||
            //    !command.CanExecute(e.ClickedItem))
            //{
            //    return;
            //}

            //command.Execute(e.ClickedItem);
        }
    }
}