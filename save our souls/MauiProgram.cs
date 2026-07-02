using Microsoft.Extensions.Logging;

namespace save_our_souls
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("SourGummy-Black.ttf", "SourGummyBlack");
                    fonts.AddFont("SourGummy-SemiBold.ttf", "SourGummySemiBold");
                });

            builder.Services.AddSingleton<Services.UserAccountDatabase>();
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddTransient<ViewModels.LoginVM>();
            builder.Services.AddTransient<ViewModels.SignUpVM>();
            builder.Services.AddTransient<Views.SignUpPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
