using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Content;
using Android.Net;
using Android.Provider;
using ZCarsDriver.Services;

namespace ZCarsDriver
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Platform.Init(this, savedInstanceState);

            CheckOverlayPermission();

            // This line is the fix 👇
            Window.SetSoftInputMode(SoftInput.AdjustResize);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            DependencyService.Get<IOverlayService>().RemoveOverlay();
        }

        private void CheckOverlayPermission()
        {
            if (!Settings.CanDrawOverlays(this))
            {
                // Show explanation dialog first (for Google Play compliance)
                new AndroidX.AppCompat.App.AlertDialog.Builder(this)
                    .SetTitle("Permission Required")
                    .SetMessage("We need permission to display ride updates on top of other apps. This helps you track your rides even if you are not in the app.")
                    .SetPositiveButton("Allow", (s, e) =>
                    {
                        var intent = new Intent(Settings.ActionManageOverlayPermission,Android.Net.Uri.Parse("package:" + PackageName));
                        StartActivityForResult(intent, 1234);
                    })
                    .SetNegativeButton("Cancel", (s, e) => { })
                    .Show();
            }
        }
    }
}
