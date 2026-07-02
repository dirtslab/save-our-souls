using System.Threading.Tasks;

namespace save_our_souls.ViewModels
{
    public class LoginVM
    {
        private readonly Services.UserAccountDatabase _userAccountDatabase;

        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public LoginVM(Services.UserAccountDatabase userAccountDatabase)
        {
            _userAccountDatabase = userAccountDatabase;
        }

        public async Task<bool> LoginUser()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
                return false;

            var userAccount = await _userAccountDatabase.GetUserAccountAsync(Username, Password);
            return userAccount != null;
        }
    }
}
