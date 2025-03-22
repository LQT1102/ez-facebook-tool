using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using EZ.FACEBOOK.TOOL.Models;
using EZ.FACEBOOK.TOOL.Services;

namespace EZ.FACEBOOK.TOOL.ViewModels
{
    public class AccountViewModel : INotifyPropertyChanged
    {
        private readonly AccountService _accountService;
        private string _accountsText;

        public string AccountsText
        {
            get => _accountsText;
            set
            {
                if (_accountsText != value)
                {
                    _accountsText = value;
                    OnPropertyChanged();
                    SaveAccountsAsync().ConfigureAwait(false);
                }
            }
        }

        public AccountViewModel(AccountService accountService)
        {
            _accountService = accountService;
            LoadAccountsAsync().ConfigureAwait(false);
        }

        private async Task LoadAccountsAsync()
        {
            var accounts = await _accountService.LoadAccountsAsync();
            AccountsText = string.Join("\n", accounts.Select(a => $"{a.Username}|{a.Password}"));
        }

        private async Task SaveAccountsAsync()
        {
            var accounts = AccountsText.Split('\n')
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line =>
                {
                    var parts = line.Split('|');
                    return new Account
                    {
                        Username = parts.Length > 0 ? parts[0].Trim() : "",
                        Password = parts.Length > 1 ? parts[1].Trim() : ""
                    };
                })
                .ToList();

            await _accountService.SaveAccountsAsync(accounts);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
} 