using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;

namespace save_our_souls
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("SourGummy-Black.ttf", "SourGummyBlack");
                    fonts.AddFont("SourGummy-SemiBold.ttf", "SourGummySemiBold");
                });

            builder.Services.AddSingleton<Services.UserAccountDatabase>();
            builder.Services.AddTransient<Views.LoginPage>();
            builder.Services.AddTransient<ViewModels.LoginVM>();
            builder.Services.AddTransient<ViewModels.SignUpVM>();
            builder.Services.AddTransient<Views.SignUpPage>();
            builder.Services.AddTransient<ViewModels.ConfigVM>();
            builder.Services.AddTransient<Views.ConfigPage>();
            builder.Services.AddTransient<ViewModels.GameVM>();
            builder.Services.AddTransient<Views.GamePage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
