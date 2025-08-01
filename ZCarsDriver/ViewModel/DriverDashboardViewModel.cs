using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using ZCars.Model.DTOs.DriverApp;
using ZCarsDriver.DPopup;
using ZCarsDriver.DPopupVM;
using ZCarsDriver.Helpers;
using ZCarsDriver.Services;
using ZCarsDriver.Services.AppService;
using ZCarsDriver.Services.Contracts;
using ZCarsDriver.Services.Session;
using ZCarsDriver.Views.Common;
using ZCarsDriver.Views.Driver;
using ZhooCars.Common;
using ZhooCars.Model.DTOs;
using ZhooSoft.Auth;
using ZhooSoft.Auth.Model;
using ZhooSoft.Controls;
using ZhooSoft.Core;

namespace ZCarsDriver.ViewModel
{
    public partial class DriverDashboardViewModel : ViewModelBase
    {
        #region Fields

        public CustomMapView CurrentMap;

        [ObservableProperty]
        private string _actionText;

        [ObservableProperty]
        private BookingRequestModel _bookingRequestModel;

        private ICallService? _callService;

        private Location _destination;

        [ObservableProperty]
        private decimal _earnings;

        private bool _endTrip;

        [ObservableProperty]
        private bool _isOnline;

        [ObservableProperty]
        private RideStatus _nextStatus;

        [ObservableProperty]
        private bool _onTrip;

        private IOsrmService _osrmService;

        [ObservableProperty]
        private RideStatus _rideStatus;

        [ObservableProperty]
        private RideTripDto _rideTrip;

        private IRideTripService? _rideTripService;

        private DriverSignalRService _signalRService;

        private bool _startRealTimeUpdate;

        private bool _startTrip;

        private ITaxiBookingService? _taxiService;

        [ObservableProperty]
        private int _tripCount;

        #endregion

        #region Constructors

        public DriverDashboardViewModel()
        {
            TripCount = 0;
            Earnings = 0.00m;
            IsOnline = false;
            ToggleOnlineStatusCommand = new AsyncRelayCommand(ToggleOnlineStatus);

            ShowTripCmd = new AsyncRelayCommand(OnShowTrip);
            GotoDashboard = new AsyncRelayCommand(OngotoDashboard);
            OpenRideOptionCmd = new AsyncRelayCommand(OpenRideOption);

            OpenCallCommand = new AsyncRelayCommand(OpenCall);
            ToggleDropdownCommand = new RelayCommand(ToggleDropdown);
            CurrentLocCommand = new AsyncRelayCommand(ShowCurrentLocation);
            OnTripAction = new AsyncRelayCommand(HandleTripAction);

            OpenSideBarCmd = new AsyncRelayCommand(OpenSideBar);

            InitializeService();
        }

        #endregion

        #region Properties

        public IAsyncRelayCommand CurrentLocCommand { get; }

        public IAsyncRelayCommand GotoDashboard { get; }

        public IAsyncRelayCommand OnTripAction { get; }

        public IAsyncRelayCommand OpenCallCommand { get; }

        public IAsyncRelayCommand OpenRideOptionCmd { get; }

        public IRelayCommand OpenSideBarCmd { get; }

        public IAsyncRelayCommand ShowTripCmd { get; }

        public IRelayCommand ToggleDropdownCommand { get; }

        public IAsyncRelayCommand ToggleOnlineStatusCommand { get; }

        #endregion

        #region Methods

        public async void InitializeMap()
        {
            try
            {
                StopRealTimeTracking();
                OnTrip = false;
                var location = await GetDriverLocation();
                if (location != null)
                {
                    CurrentMap.MapElements.Clear();
                    CurrentMap.Pins.Clear();
                    var position = new Location(location.Latitude, location.Longitude);
                    var pin = new CustomPin
                    {
                        Label = "Your Location",
                        Type = PinType.Place,
                        Location = position,
                        Address = "Location",
                        ImageSource = "car_icon.png"
                    };

                    CurrentMap.Pins.Add(pin);

                    CurrentMap.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(1)));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting location: {ex.Message}");
            }
        }

        public async Task InitSignalR()
        {
            _signalRService = ServiceHelper.GetService<DriverSignalRService>();
            if (_signalRService != null)
            {
                _signalRService.Initialize(UserDetails.Instance.CurrentUser.UserId);
                await _signalRService.ConnectAsync();
                _signalRService.StartSendingLocation();
            }
        }

        public async override void OnAppearing()
        {
            base.OnAppearing();
            await GetCurrentRide();
            if (!IsLoaded && AppHelper.CurrentRide != null)
            {
                Task.Run(() => InitSignalR());

            }
            await RefreshPage();

            //Get Driver details API

            await ServiceHelper.GetService<IUserSessionManager>().SetUserPreference("CurrentModule", UserRoles.Driver.ToString());

            IsLoaded = true;
        }

        public async Task<double?> PlotRouteOnMap(double startLat, double startLng, double endLat, double endLng)
        {
            try
            {
                // Get encoded polyline from OSRM
                var result = await _osrmService.GetRoutePoints(startLat, startLng, endLat, endLng);

                if (result.Locations == null || result.Locations.Count() == 0)
                {
                    Console.WriteLine("No route found!");
                    return null;
                }

                // Clear previous routes
                CurrentMap.MapElements.Clear();

                // Draw the polyline
                var polyline = new Polyline
                {
                    StrokeColor = Colors.Green,
                    StrokeWidth = 5
                };

                foreach (var loc in result.Locations)
                {
                    polyline.Geopath.Add(loc);
                }

                CurrentMap.MapElements.Add(polyline);

                CurrentMap.Pins.Clear();

                var pin1 = new CustomPin
                {
                    Label = "Your Location",
                    Type = PinType.Place,
                    Location = new Location(result.Locations.First().Latitude, result.Locations.First().Longitude),
                    Address = "Location",
                    ImageSource = "car_icon.png"
                };

                var pin2 = new CustomPin
                {
                    Label = "Your Location",
                    Type = PinType.Generic,
                    Location = new Location(result.Locations.Last().Latitude, result.Locations.Last().Longitude),
                    Address = "Location",
                    ImageSource = "pin.png"
                };

                CurrentMap.Pins.Add(pin1);

                CurrentMap.Pins.Add(pin2);

                // Center the map
                CurrentMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Location(startLat, startLng), Distance.FromKilometers(10)));

                //var bearing = PolylineDecoder.CalculateBearing(result.Locations.First(), result.Locations.Last());

                //await CurrentMap.RotateTo(bearing, 500, Easing.BounceIn);

                return result.Distance;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error plotting route: {ex.Message}");
            }
            return null;
        }

        public async void TriggerAfterDelayAsync()
        {
        }

        internal async Task RefreshPage()
        {
            if (AppHelper.CurrentRide != null && AppHelper.CurrentRide.CurrentStatus == RideStatus.Assigned)
            {
                OnTrip = true;
                ActionText = AppHelper.CurrentRide.NextStatus().ToString();
                await OnStartPickup();
            }
            else if (AppHelper.CurrentRide != null && AppHelper.CurrentRide.CurrentStatus == RideStatus.Reached)
            {
                OnTrip = false;
                ActionText = AppHelper.CurrentRide.NextStatus().ToString();
            }
            else if (AppHelper.CurrentRide != null && AppHelper.CurrentRide.CurrentStatus == RideStatus.Started)
            {
                OnTrip = true;
                ActionText = AppHelper.CurrentRide.NextStatus().ToString();
                await OnStartTrip();
            }
            else if (AppHelper.CurrentRide != null && AppHelper.CurrentRide.CurrentStatus == RideStatus.Cancelled)
            {
                AppHelper.CurrentRide = null;
                InitializeMap();
            }
            else if (AppHelper.CurrentRide == null)
            {
                InitializeMap();
                StopRealTimeTracking();
            }
        }

        private async void Geolocation_LocationChanged(object? sender, GeolocationLocationChangedEventArgs e)
        {
            await PlotRouteOnMap(e.Location.Latitude, e.Location.Longitude, _destination.Latitude, _destination.Longitude);
        }

        private async Task GetCurrentRide()
        {
        }

        private async Task<Location> GetDriverLocation()
        {
            try
            {
                var location = await Geolocation.GetLocationAsync() ?? await Geolocation.GetLastKnownLocationAsync();
                return location;
            }
            catch (Exception ex)
            {
                await _alertService.ShowAlert("Error", "Enable Location", "Ok");
            }
            return null;
        }

        private async Task HandleTripAction()
        {
            if (AppHelper.CurrentRide.NextStatus() == RideStatus.Reached)
            {
                OnTrip = true;
                await OnReachedPickup();
                AppHelper.CurrentRide.CurrentStatus = RideStatus.Reached;
                ActionText = AppHelper.CurrentRide.NextStatus().ToString();
            }
            else if (AppHelper.CurrentRide.NextStatus() == RideStatus.Started)
            {
                OnTrip = true;
                await OnInitializeStart();
            }
            else if (AppHelper.CurrentRide.NextStatus() == RideStatus.Completed)
            {
                await OnCompleteTrip();
            }
        }

        private void InitializeService()
        {
            _osrmService = ServiceHelper.GetService<IOsrmService>();
            _callService = ServiceHelper.GetService<ICallService>();
            _rideTripService = ServiceHelper.GetService<IRideTripService>();
            _taxiService = ServiceHelper.GetService<ITaxiBookingService>();
        }

        private async Task OnCompleteTrip()
        {
            var pp = new OnStartOtpPopup();

            if (pp.BindingContext is OnStartOtpViewModel vm)
            {
                vm.Description = "Please Enter the End Trip OTP";
                vm.ShowOtp = true;
            }

            var result = await _navigationService.OpenPopup(pp);

            if (result is string otp && otp.Length == 4)
            {
                var response = await _signalRService.NotifyCompleteTrip(otp);

                if (response)
                {
                    await OnEndTripAction();
                }
                else
                {
                    await _alertService.ShowAlert("Info", "Invalid OTP", "Ok");
                }
            }
        }

        private async Task OnEndTripAction()
        {
            IsBusy = true;
            StopRealTimeTracking();
            OnTrip = false;
            AppHelper.CurrentRide.CurrentStatus = RideStatus.Completed;
            OnTrip = false;
            var rideTripInfo = await _rideTripService.EndTripAsync(new EndTripDto
            {
                Distance = 1,
                EndLatitude = 10.09,
                EndLongitude = 2002.90,
                RideTripId = 1,
                StartLatitude = 22,
                StartLongitude = 22
            });
            AppHelper.CurrentRide = null;
            InitializeMap();
            IsBusy = false;

            var pp = new OnStartOtpPopup();

            if (pp.BindingContext is OnStartOtpViewModel vm1)
            {
                vm1.ShowEndTrip = false;
                vm1.ShowOtp = false;
                vm1.ShowRideSuccess = true;
            }

            await _navigationService.OpenPopup(pp);
        }

        private async Task OngotoDashboard()
        {
            var result = await _alertService.ShowConfirmation("Info", "Are you Leaving the driver dashboard?", "Ok", "Cancel");

            if (result)
            {
                ServiceHelper.GetService<IMainAppNavigation>().NavigateToMain();
            }
        }

        private async Task OnInitializeStart()
        {
            IsBusy = true;
            var result = await _navigationService.OpenPopup(new OnStartOtpPopup());

            if (result is string otp && otp.Length == 4)
            {
                var response = await _signalRService.NotifyStartTrip(otp);

                if (response)
                {
                    await OnStartTrip();
                }
                else
                {
                    await _alertService.ShowAlert("Info", "Invalid OTP", "Ok");
                }
            }
            IsBusy = false;
        }

        private async Task OnReachedPickup()
        {
            IsBusy = true;
            await _rideTripService.ReachPickupAsync(new UpdateTripStatusDto
            {
                RideTripId = 1
            });
            await _signalRService.NotifyPickupReached();
            _rideStatus = RideStatus.Reached;
            StopRealTimeTracking();
            IsBusy = false;
        }

        private async Task OnShowTrip()
        {
        }

        private async Task OnStartPickup()
        {
            IsBusy = true;
            _startRealTimeUpdate = true;
            _destination = new Location(AppHelper.CurrentRide.BookingRequest.PickupLatitude, AppHelper.CurrentRide.BookingRequest.PickupLongitude);
            OnTrip = true;
            await StartRealTimeTracking();
            IsBusy = false;
        }

        private async Task OnStartTrip()
        {
            _startRealTimeUpdate = true;
            _startTrip = true;
            AppHelper.CurrentRide.CurrentStatus = RideStatus.Started;
            ActionText = AppHelper.CurrentRide.NextStatus().ToString();
            OnTrip = true;
            _destination = new Location(AppHelper.CurrentRide.BookingRequest.DropLatitude, AppHelper.CurrentRide.BookingRequest.DropLongitude);
            await StartRealTimeTracking();
        }

        private async Task OpenCall()
        {
            // Logic to open phone call
            await _callService?.MakePhoneCall("8344273152");
        }

        private async Task OpenRideOption()
        {
            var SelectedOption = await _navigationService.OpenPopup(new TripOptionPopup());

            if (SelectedOption is PopupEnum result)
            {
                if (result == PopupEnum.TripDetails)
                {
                    await _navigationService.PushAsync(ServiceHelper.GetService<TripDetailsPage>());
                }
                if (result == PopupEnum.CallRide)
                {
                    await OpenCall();
                }
                if (result == PopupEnum.CancelRide)
                {
                    await _navigationService.PushAsync(ServiceHelper.GetService<CancelTripPage>());
                }
            }
        }

        private async Task OpenSideBar()
        {
            IsBusy = true;
            var result = await _navigationService.OpenPopup(new CommonMenu());

            await Task.Delay(100);
            IsBusy = false;
        }

        private async Task ShowCurrentLocation()
        {
            var location = await GetDriverLocation();
            if (location != null)
            {
                CurrentMap.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(1)));
            }
        }

        private async Task StartRealTimeTracking()
        {
            var location = await Geolocation.GetLastKnownLocationAsync();

            if (location != null)
            {
                await PlotRouteOnMap(location.Latitude, location.Longitude, _destination.Latitude, _destination.Longitude);
                if (Geolocation.Default.IsListeningForeground)
                {
                    Geolocation.Default.StopListeningForeground();
                }

                await Geolocation.StartListeningForegroundAsync(new GeolocationListeningRequest
                { DesiredAccuracy = GeolocationAccuracy.Best, MinimumTime = TimeSpan.FromSeconds(5) });

                Geolocation.LocationChanged -= Geolocation_LocationChanged;
                Geolocation.LocationChanged += Geolocation_LocationChanged;
            }
        }

        private void StopRealTimeTracking()
        {
            _startRealTimeUpdate = false;
            Geolocation.LocationChanged -= Geolocation_LocationChanged;

            if (Geolocation.Default.IsListeningForeground)
            {
                Geolocation.Default.StopListeningForeground();
            }
        }

        private void ToggleDropdown()
        {
            // Logic to show/hide dropdown
            Console.WriteLine("Dropdown toggled");
        }

        private async Task ToggleOnlineStatus()
        {
            //API call to on or off online
            await _alertService.ShowAlert("message", "online", "ok");

            if (IsOnline)
            {
                Task.Run(() => { InitSignalR(); TriggerAfterDelayAsync(); });
            }
        }

        #endregion
    }
}
