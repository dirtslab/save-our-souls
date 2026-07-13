using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Widget;

namespace save_our_souls
{
    [BroadcastReceiver(Enabled = true, Exported = true)]
    [IntentFilter(new[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
    [MetaData("android.appwidget.provider", Resource = "@xml/app_widget_info")]
    public class LauncherWidgetProvider : AppWidgetProvider
    {
        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            foreach (var appWidgetId in appWidgetIds)
            {
                var views = new RemoteViews(context.PackageName, Resource.Layout.app_widget);

                var intent = new Intent(context, typeof(MainActivity));
                intent.AddFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);

                var pendingIntent = PendingIntent.GetActivity(
                    context,
                    appWidgetId,
                    intent,
                    PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

                views.SetOnClickPendingIntent(Resource.Id.widget_root, pendingIntent);
                views.SetOnClickPendingIntent(Resource.Id.widget_title, pendingIntent);
                views.SetOnClickPendingIntent(Resource.Id.widget_subtitle, pendingIntent);

                appWidgetManager.UpdateAppWidget(appWidgetId, views);
            }
        }
    }
}
