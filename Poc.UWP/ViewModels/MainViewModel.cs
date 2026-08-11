using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using Poc.UWP.Models;

namespace Poc.UWP.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private string _account;
        private string _accountFeatureFlags;

        public MainViewModel()
        {
            System.Diagnostics.Debug.WriteLine($"jsonString");
            LoadCommand = new RelayCommand(LoadCommandBehavior);
        }

        public IRelayCommand LoadCommand { get; set; }

        public string Account
        {
            get => _account;
            set => SetProperty(ref _account, value);
        }

        public string AccountFeatureFlags
        {
            get => _accountFeatureFlags;
            set => SetProperty(ref _accountFeatureFlags, value);
        }

        private void LoadCommandBehavior()
        {
            System.Diagnostics.Debug.WriteLine($"jsonString");


            Account = "Account"; // System.Text.Json.JsonSerializer.Serialize(account);
            AccountFeatureFlags = "Account"; // System.Text.Json.JsonSerializer.Serialize(accountFeatureFlags);
			System.Diagnostics.Debug.WriteLine($"jsonString {Account} {AccountFeatureFlags}");
        }
    }
}