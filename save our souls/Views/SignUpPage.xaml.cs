using save_our_souls.ViewModels;

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
            await signUpVM.SignUpUser();
        }
    }
}
