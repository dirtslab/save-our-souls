namespace save_our_souls
{
    /// <summary>
    /// Static bridge that lets Android-platform code signal a widget-initiated
    /// game launch to the cross-platform MAUI layer.
    /// </summary>
    public static class WidgetLaunchState
    {
        /// <summary>
        /// Set to true by MainActivity when the app is opened from the
        /// Game Config widget. The LoginPage checks this on appearing and
        /// navigates directly to the GamePage when it is true.
        /// </summary>
        public static bool PendingLaunch { get; set; }
    }
}
