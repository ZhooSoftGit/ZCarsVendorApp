using Android.Content;
using Android.Locations;
using Android.Provider;
using ZCarsDriver.PlatformHelper;

namespace ZCarsDriver.Platforms.Droid.Helpers
{
    public class LocationHelperAndroid : ILocationHelper
    {
        public bool IsLocationEnabled()
        {
            var locationManager = (LocationManager)Microsoft.Maui.ApplicationModel.Platform.CurrentActivity.GetSystemService(Context.LocationService);
            return locationManager.IsProviderEnabled(LocationManager.GpsProvider) ||
                   locationManager.IsProviderEnabled(LocationManager.NetworkProvider);
        }

        public void OpenLocationSettings()
        {
            Intent intent = new Intent(Settings.ActionLocationSourceSettings);
            Microsoft.Maui.ApplicationModel.Platform.CurrentActivity.StartActivity(intent);
        }
    }
}
