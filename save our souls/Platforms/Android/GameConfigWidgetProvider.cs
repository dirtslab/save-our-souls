using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Widget;
using Resource = save_our_souls.Resource;

namespace save_our_souls
{
    [BroadcastReceiver(Enabled = true, Exported = true)]
    [IntentFilter(new[]
    {
        "android.appwidget.action.APPWIDGET_UPDATE",
        GameConfigWidgetProvider.ActionModeToggle,
        GameConfigWidgetProvider.ActionSizeDecrease,
        GameConfigWidgetProvider.ActionSizeIncrease
    })]
    [MetaData("android.appwidget.provider", Resource = "@xml/game_config_widget_info")]
    public class GameConfigWidgetProvider : AppWidgetProvider
    {
        public const string ActionModeToggle   = "save_our_souls.ACTION_CONFIG_MODE_TOGGLE";
        public const string ActionSizeDecrease = "save_our_souls.ACTION_CONFIG_SIZE_DEC";
        public const string ActionSizeIncrease = "save_our_souls.ACTION_CONFIG_SIZE_INC";

        private const string PrefsName  = "game_config_widget_prefs";
        private const string KeyMode    = "widget_game_mode";   // true = singleplayer
        private const string KeySize    = "widget_game_size";
        private const int    MinSize    = 3;
        private const int    MaxSize    = 10;
        private const int    DefaultSize = 5;

        // ------------------------------------------------------------------ //
        // AppWidgetProvider overrides
        // ------------------------------------------------------------------ //

        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            foreach (var id in appWidgetIds)
                UpdateWidget(context, appWidgetManager, id);
        }

        public override void OnReceive(Context context, Intent intent)
        {
            base.OnReceive(context, intent);

            var action = intent.Action;
            if (action == null)
                return;

            var prefs  = context.GetSharedPreferences(PrefsName, Android.Content.FileCreationMode.Private)!;
            var editor = prefs.Edit()!;

            switch (action)
            {
                case ActionModeToggle:
                    bool current = prefs.GetBoolean(KeyMode, true);
                    editor.PutBoolean(KeyMode, !current);
                    editor.Apply();
                    break;

                case ActionSizeDecrease:
                    int sizeD = prefs.GetInt(KeySize, DefaultSize);
                    editor.PutInt(KeySize, Math.Max(MinSize, sizeD - 1));
                    editor.Apply();
                    break;

                case ActionSizeIncrease:
                    int sizeI = prefs.GetInt(KeySize, DefaultSize);
                    editor.PutInt(KeySize, Math.Min(MaxSize, sizeI + 1));
                    editor.Apply();
                    break;

                default:
                    return; // nothing to refresh
            }

            // Refresh all instances of this widget after a state change.
            var manager = AppWidgetManager.GetInstance(context)!;
            var ids     = manager.GetAppWidgetIds(
                new ComponentName(context, Java.Lang.Class.FromType(typeof(GameConfigWidgetProvider))))!;
            foreach (var id in ids)
                UpdateWidget(context, manager, id);
        }

        // ------------------------------------------------------------------ //
        // Helpers
        // ------------------------------------------------------------------ //

        private static void UpdateWidget(Context context, AppWidgetManager manager, int widgetId)
        {
            var prefs      = context.GetSharedPreferences(PrefsName, Android.Content.FileCreationMode.Private)!;
            bool isSingle  = prefs.GetBoolean(KeyMode, true);
            int  size      = prefs.GetInt(KeySize, DefaultSize);

            var views = new RemoteViews(context.PackageName!, Resource.Layout.game_config_widget);

            // ---- Labels ----
            views.SetTextViewText(Resource.Id.config_widget_mode_btn,
                isSingle ? "Mode: Singleplayer" : "Mode: Multiplayer");
            views.SetTextViewText(Resource.Id.config_widget_size_label, $"{size}x{size}");

            // ---- Mode toggle ----
            views.SetOnClickPendingIntent(Resource.Id.config_widget_mode_btn,
                MakeBroadcastIntent(context, ActionModeToggle, widgetId));

            // ---- Size controls ----
            views.SetOnClickPendingIntent(Resource.Id.config_widget_size_dec,
                MakeBroadcastIntent(context, ActionSizeDecrease, widgetId));
            views.SetOnClickPendingIntent(Resource.Id.config_widget_size_inc,
                MakeBroadcastIntent(context, ActionSizeIncrease, widgetId));

            // ---- Start Game ----
            views.SetOnClickPendingIntent(Resource.Id.config_widget_start_btn,
                MakeStartGameIntent(context, widgetId, isSingle, size));

            manager.UpdateAppWidget(widgetId, views);
        }

        private static PendingIntent MakeBroadcastIntent(Context context, string action, int widgetId)
        {
            var intent = new Intent(context, typeof(GameConfigWidgetProvider));
            intent.SetAction(action);
            // Use widgetId as request code so each widget instance gets a unique PendingIntent.
            return PendingIntent.GetBroadcast(
                context,
                widgetId + action.GetHashCode(),
                intent,
                PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable)!;
        }

        private static PendingIntent MakeStartGameIntent(Context context, int widgetId, bool isSingleplayer, int gameSize)
        {
            var intent = new Intent(context, typeof(MainActivity));
            intent.SetAction(Intent.ActionMain);
            intent.AddCategory(Intent.CategoryLauncher);
            intent.AddFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
            intent.PutExtra("widget_launch",   true);
            intent.PutExtra("widget_game_mode", isSingleplayer);
            intent.PutExtra("widget_game_size", gameSize);

            return PendingIntent.GetActivity(
                context,
                widgetId + 9999,
                intent,
                PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable)!;
        }
    }
}
