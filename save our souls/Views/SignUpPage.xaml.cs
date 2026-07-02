namespace save_our_souls.Views;

public partial class SignUpPage : ContentPage
{
	public SignUpPage()
	{
		InitializeComponent();
	}

	private async void OnCreateAccountClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("..", false);
	}
}
