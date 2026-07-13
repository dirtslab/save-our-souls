using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace save_our_souls
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HandleWidgetIntent(Intent);
        }

        protected override void OnNewIntent(Intent? intent)
        {
            base.OnNewIntent(intent);
            HandleWidgetIntent(intent);
        }

        private static void HandleWidgetIntent(Intent? intent)
        {
            if (intent == null || !intent.GetBooleanExtra("widget_launch", false))
                return;

            bool isSingleplayer = intent.GetBooleanExtra("widget_game_mode", true);
            int  gameSize       = intent.GetIntExtra("widget_game_size", 5);

            // Persist selections so ConfigVM/GameVM can read them normally.
            Preferences.Default.Set("GameMode", isSingleplayer);
            Preferences.Default.Set("GameSize", gameSize);

            // Signal the MAUI layer to skip login and go straight to the game.
            WidgetLaunchState.PendingLaunch = true;
        }
    }
}
