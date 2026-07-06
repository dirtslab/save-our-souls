using save_our_souls.Views;

namespace save_our_souls
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Views.SignUpPage), typeof(Views.SignUpPage));
            Routing.RegisterRoute(nameof(Views.ConfigPage), typeof(Views.ConfigPage));
            Routing.RegisterRoute(nameof(Views.GamePage), typeof(Views.GamePage));
        }
    }
}
