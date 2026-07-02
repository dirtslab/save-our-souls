using save_our_souls.ViewModels;

namespace save_our_souls;

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
				await DisplayAlert("Login Successful", "Welcome back!", "OK");
			else
				await DisplayAlert("Login Failed", "Invalid username or password.", "OK");
		}
	}

	private async void OnCreateAccountClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(Views.SignUpPage));
	}
}