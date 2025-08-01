using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Timers;
using ZCars.Model.Common;
using ZCars.Model.DTOs.DriverApp;
using ZCarsDriver.Core.Stoage;
using ZCarsDriver.Helpers;
using ZCarsDriver.Services.Contracts;
using ZCarsDriver.UIModel;
using ZhooCars.Common;
using ZhooSoft.Core;

namespace ZCarsDriver.DPopup
{
    public partial class BookingRequestViewModel : ViewModelBase
    {
        #region Constants

        private const int TotalTime = 10;

        #endregion

        #region Fields

        [ObservableProperty]
        private BookingRequestModel _bookingRequest;

        [ObservableProperty]
        private double _progressValue;

        private System.Timers.Timer _timer;

        #endregion

        #region Constructors

        public BookingRequestViewModel()
        {
            AcceptCommand = new AsyncRelayCommand(OnAccept);
            RejectCommand = new AsyncRelayCommand(OnReject);
            _taxiBookingService = ServiceHelper.GetService<ITaxiBookingService>();
            InitiateTimer();
        }

        #endregion

        #region Properties

        public IAsyncRelayCommand AcceptCommand { get; }

        public BookingRequestPopup CurrentPopup { get; set; }

        public IAsyncRelayCommand RejectCommand { get; }

        private readonly ITaxiBookingService? _taxiBookingService;

        public int TimerValue { get; set; }

        #endregion

        #region Methods

        public override void OnNavigatedTo()
        {
            if (NavigationParams != null && NavigationParams.ContainsKey("RequestModel"))
            {
                BookingRequest = NavigationParams["RequestModel"] as BookingRequestModel;
            }
        }

        private void InitiateTimer()
        {
            TimerValue = TotalTime;
            ProgressValue = TimerValue;
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += OnTimerElapsed;
            _timer.Start();
        }

        private async Task OnAccept()
        {
            IsBusy = true;
            _timer.Dispose();
            var result = await _taxiBookingService.AcceptRideAsync(new ZhooCars.Model.DTOs.AcceptRideRequest
            {
                RideRequestId = BookingRequest.BoookingRequestId,
                VehicleId = 12,
                DriverId = 123
            });
            IsBusy = false;

            if (result.IsSuccess)
            {
                result.Data.RideRequestId = BookingRequest.BoookingRequestId;
                var ride = new CurrentRide
                {
                    BookingRequest = BookingRequest,
                    RideDetails = result.Data,
                    CurrentStatus = RideStatus.Assigned
                };

                RideStorageService.Save(ride);
                await CurrentPopup.CloseAsync(RideStatus.Assigned);
            }
            else
            {
                await _alertService.ShowAlert("Error", "Sorry, Not able to assign. Please try again", "ok");
            }
        }

        private async Task OnReject()
        {
            _timer.Dispose();
            // Perform logic for rejecting the ride
            Application.Current.MainPage.DisplayAlert("Info", "Ride Rejected", "OK");
            await CurrentPopup.CloseAsync(RideStatus.Cancelled);
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            TimerValue--;
            ProgressValue = TimerValue;
            OnPropertyChanged(nameof(TimerValue));
            OnPropertyChanged(nameof(ProgressValue));

            if (TimerValue <= 0)
            {
                _timer.Stop();
                CurrentPopup.CloseAsync();
            }
        }

        #endregion
    }
}
