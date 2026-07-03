using CommunityToolkit.Maui.Alerts;
using save_our_souls.ViewModels;
using static System.Net.Mime.MediaTypeNames;

namespace save_our_souls.Views;

public partial class SignUpPage : ContentPage
{
    SignUpVM signUpVM;

    public SignUpPage(SignUpVM signUpVM)
    {
        InitializeComponent();
        this.signUpVM = signUpVM;
        BindingContext = signUpVM;
    }

    private async void OnCreateAccountClicked(object sender, EventArgs e)
    {
        if (signUpVM != null)
        {
            try
            {
                bool success = await signUpVM.SignUpUser();
                if (!success)
                {
                    await DisplayAlertAsync("Sign Up Failed", "Please enter a valid username and password.", "OK");
                }

                var toast = Toast.Make("Account was successfully created!", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);

                await toast.Show();

                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Sign Up Failed", ex.Message, "OK");
            }

        }
    }

    private void OnShowPasswordToggle(object sender, EventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
        ToggleBtn.Source = PasswordEntry.IsPassword ? "show_icon.png" : "hide_icon.png";
    }
}
