using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ZCars.Model.DTOs.DriverApp;
using ZCarsDriver.Helpers;
using ZhooCars.Model.DTOs;
using ZhooSoft.Core;

namespace ZCarsDriver.ViewModel
{
    public partial class TripDetailsViewModel : ViewModelBase
    {
        #region Fields

        [ObservableProperty]
        private BookingRequestModel _bookingRequest;

        [ObservableProperty]
        private RideTripDto _rideTripDto;

        #endregion

        #region Constructors

        public TripDetailsViewModel()
        {
            PageTitleName = "Trip Details";
        }

        #endregion

        #region Properties

        public IRelayCommand GoBackCommand { get; }

        #endregion

        #region Methods

        public override void OnAppearing()
        {
            base.OnAppearing();

            if (AppHelper.CurrentRide != null)
            {
                BookingRequest = AppHelper.CurrentRide.BookingRequest;
                RideTripDto = AppHelper.CurrentRide.RideDetails;
            }
        }

        #endregion
    }
}
