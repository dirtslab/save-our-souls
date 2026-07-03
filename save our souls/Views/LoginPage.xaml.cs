using save_our_souls.ViewModels;

namespace save_our_souls.Views;

public partial class LoginPage : ContentPage
{
    LoginVM loginVM;

    public LoginPage(LoginVM loginVM)
    {
        InitializeComponent();
        this.loginVM = loginVM;
        BindingContext = loginVM;
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        if (loginVM != null)
        {
            bool success = await loginVM.LoginUser();
            if (success)
            {
                await Shell.Current.GoToAsync(nameof(Views.ConfigPage));
            }
            else
                await DisplayAlertAsync("Login Failed", "Invalid username or password.", "OK");
        }
    }

    private async void OnCreateAccountClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(Views.SignUpPage));
    }

    private void OnShowPasswordToggle(object sender, EventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
        ToggleBtn.Source = PasswordEntry.IsPassword ? "show_icon.png" : "hide_icon.png";
    }
}