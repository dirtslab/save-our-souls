using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace save_our_souls.ViewModels
{
    static class ConfigColors
    {
        public static readonly List<Color> ColorOptions = new List<Color>
        {
            Colors.Red,
            Colors.Green,
            Colors.Blue,
            Colors.Yellow,
            Colors.Purple,
            Colors.Orange
        };
    }

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

        private string _username = string.Empty;
        public string Username
        {
            get => _username;
            private set
            {
                if (_username == value)
                    return;
                _username = value;
                OnPropertyChanged();
            }
        }

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
