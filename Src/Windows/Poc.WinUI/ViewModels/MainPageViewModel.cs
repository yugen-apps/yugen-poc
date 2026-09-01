using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Poc_Lib_WinUI;


namespace Poc_WinUI.ViewModels
{
	/// <summary>
	/// Sample ViewModel using CommunityToolkit.Mvvm partial property syntax.
	/// Uses <see cref="ObservableProperty"/> for change notification and
	/// <see cref="RelayCommand"/> for command binding.
	/// </summary>
	public partial class MainPageViewModel : ObservableObject
	{
		[ObservableProperty]
		public partial string Greeting { get; set; } = TestHelper.GetTestString();

		[ObservableProperty]
		public partial int Counter { get; set; }

		[RelayCommand]
		private void Increment()
		{
			Counter++;
		}

		[RelayCommand]
		private void Decrement()
		{
			Counter--;
		}
	}
}
