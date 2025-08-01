﻿using CommunityToolkit.Mvvm.Input;
using ZhooSoft.Core;

namespace ZCarsDriver.ViewModel
{
    public class CustomMapWebViewModel : ViewModelBase
    {
        #region Fields

        private double startLat, startLng, destinationLat, destinationLng;

        #endregion

        #region Constructors

        public CustomMapWebViewModel()
        {
            OpenDashBoardCommand = new AsyncRelayCommand(OpenDashboard);
        }

        #endregion

        #region Properties

        public IAsyncRelayCommand OpenDashBoardCommand { get; }

        #endregion

        #region Methods

        public async Task OpenMapAsync(double startLat, double startLng, double endLat, double endLng)
        {
            string uri = string.Empty;

            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                uri = $"https://www.google.com/maps/dir/?api=1&origin={startLat},{startLng}&destination={endLat},{endLng}&travelmode=driving";
            }
            else if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                uri = $"http://maps.apple.com/?saddr={startLat},{startLng}&daddr={endLat},{endLng}&dirflg=d";
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Unsupported Platform", "Maps are not supported on this platform.", "OK");
                return;
            }

            if (await Launcher.CanOpenAsync(uri))
            {
                await Launcher.OpenAsync(uri);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Unable to open Maps.", "OK");
            }
        }

        private async Task GetLocation()
        {
            var location = await Geolocation.GetLastKnownLocationAsync() ?? await Geolocation.GetLocationAsync();

            if (location != null)
            {
                startLat = location.Latitude;
                startLng = location.Longitude;

                destinationLat = 12.9716; // Example: Bangalore
                destinationLng = 77.5946;
            }
        }

        private async Task OpenDashboard()
        {
            await GetLocation();
            await OpenMapAsync(startLat, startLng, destinationLat, destinationLng);
        }

        #endregion
    }
}
