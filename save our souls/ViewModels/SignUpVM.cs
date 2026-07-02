using System.Threading.Tasks;

namespace save_our_souls.ViewModels
{
    public class SignUpVM
    {
        private readonly Services.UserAccountDatabase _userAccountDatabase;

        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public SignUpVM(Services.UserAccountDatabase userAccountDatabase)
        {
            _userAccountDatabase = userAccountDatabase;
        }

        public async Task SignUpUser()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
                return;

            var userAccount = new Models.UserAccountModel
            {
                Name = string.Empty,
                Username = Username,
                Password = Password
            };

            await _userAccountDatabase.AddUserAccountAsync(userAccount);
        }
    }
}
