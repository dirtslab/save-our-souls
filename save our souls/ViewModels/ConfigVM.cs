using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace save_our_souls.ViewModels
{
    public class ConfigVM : INotifyPropertyChanged
    {
        private readonly Services.UserAccountDatabase _userAccountDatabase;

        private string? _profileImageUri;
        public string? ProfileImageUri
        {
            get => _profileImageUri;
            private set
            {
                if (_profileImageUri == value)
                    return;

                _profileImageUri = value;
                OnPropertyChanged();
            }
        }

        public string Username { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ConfigVM(Services.UserAccountDatabase userAccountDatabase)
        {
            _userAccountDatabase = userAccountDatabase;
        }

        public async Task LoadProfileImageAsync()
        {
            var username = Preferences.Default.Get<string?>("CurrentUsername", null);
            if (string.IsNullOrWhiteSpace(username))
            {
                ProfileImageUri = null;
                return;
            }

            
            var userAccount = await _userAccountDatabase.GetUserAccountByUsernameAsync(username);

            if (userAccount == null) return;

            ProfileImageUri = userAccount?.Photo;
            if (!string.IsNullOrWhiteSpace(userAccount?.Name))
            {
                Username = userAccount?.Name;
            }
            else
            {
                Username = username;
            }
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
