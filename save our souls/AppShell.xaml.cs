namespace save_our_souls
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Views.SignUpPage), typeof(Views.SignUpPage));
        }
    }
}
