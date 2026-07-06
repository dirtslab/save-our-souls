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
            UsernameEntry.Unfocus();
            PasswordEntry.Unfocus();

            UsernameEntry.IsEnabled = false;
            PasswordEntry.IsEnabled = false;

            try
            {
                bool success = await loginVM.LoginUser();
                if (success)
                {
                    await Shell.Current.GoToAsync(nameof(Views.ConfigPage));
                }
                else
                {
                    await DisplayAlertAsync("Login Failed", "Invalid username or password.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Login Failed", ex.Message, "OK");
            }
            finally
            {
                UsernameEntry.IsEnabled = true;
                PasswordEntry.IsEnabled = true;
            }
        }
    }

    private async void OnCreateAccountClicked(object sender, EventArgs e)
    {
        UsernameEntry.Unfocus();
        PasswordEntry.Unfocus();
        UsernameEntry.IsEnabled = false;
        PasswordEntry.IsEnabled = false;
        try
        {
            await Shell.Current.GoToAsync(nameof(Views.SignUpPage));
        }
        finally
        {
            UsernameEntry.IsEnabled = true;
            PasswordEntry.IsEnabled = true;
        }
    }

    private void OnShowPasswordToggle(object sender, EventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
        ToggleBtn.Source = PasswordEntry.IsPassword ? "show_icon.png" : "hide_icon.png";
    }
}