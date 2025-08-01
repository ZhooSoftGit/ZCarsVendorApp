using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using ZCarsDriver.Core;
using ZCarsDriver.Helpers;
using ZCarsDriver.Services.Contracts;
using ZCarsDriver.Services.Session;
using ZCarsDriver.Views.Driver;
using ZhooCars.Common;
using ZhooSoft.Core;

namespace ZCarsDriver.ViewModel
{
    public partial class HomeViewModel : ViewModelBase
    {
        #region Fields

        private readonly IDriverService? _driverService;

        private readonly IUserSessionManager _sessionManager;

        private readonly IUserService _userService;

        [ObservableProperty]
        private string userName = "User Name";

        [ObservableProperty]
        private string userRole = "User Role";

        #endregion

        #region Constructors

        public HomeViewModel()
        {
            ShowRideHistoryCommand = new RelayCommand(ShowRideHistory);
            ShowPaymentCommand = new RelayCommand(ShowPayment);
            OpenNotificationCommand = new RelayCommand(OpenNotification);
            OpenReferFriendCommand = new RelayCommand(OpenReferFriend);
            OpenSettingsCommand = new RelayCommand(OpenSettings);
            LogoutCommand = new RelayCommand(Logout);
            TileClickCmd = new RelayCommand<string>(OnTileClicked);

            _userService = ServiceHelper.GetService<IUserService>();
            _driverService = ServiceHelper.GetService<IDriverService>();
        }

        #endregion

        #region Properties

        public ICommand LogoutCommand { get; }

        public ICommand OpenNotificationCommand { get; }

        public ICommand OpenReferFriendCommand { get; }

        public ICommand OpenSettingsCommand { get; }

        public ICommand ShowPaymentCommand { get; }

        public ICommand ShowRideHistoryCommand { get; }

        public ICommand TileClickCmd { get; }

        #endregion

        #region Methods

        public override async void OnAppearing()
        {
            IsBusy = true;
            base.OnAppearing();
            if (!AppHelper.InitialLoadDone)
            {
                await LoadUserdata();
            }
            IsBusy = false;
        }

        private async Task LoadUserdata()
        {
            var result = await _driverService.GetDriverProfile();

            if (result.IsSuccess && result.Data != null)
            {
                AppHelper.InitialLoadDone = true;
                if (result.Data.UserDetail != null)
                {
                    GenericPreferenceService.Instance.Set(GlobalConstants.UserDetailData, result.Data.UserDetail);
                }
                if (result.Data.Documents != null && result.Data.Documents.Count > 0)
                {
                    GenericPreferenceService.Instance.Set(GlobalConstants.UserDocs, result.Data.Documents);
                }

                if (result.Data.DriverDetail != null)
                {
                    GenericPreferenceService.Instance.Set(GlobalConstants.DriverData, result.Data.DriverDetail);
                }

                if (result.Data.DriverVehicleLink != null)
                {
                    GenericPreferenceService.Instance.Set(GlobalConstants.VehicleLinkData, result.Data.DriverVehicleLink);
                }
            }
        }

        private void Logout()
        {
        }

        private async void OnTileClicked(string option)
        {
            if (string.IsNullOrEmpty(option))
                return;
            option = option.Replace(" ", "");
            var nvparam = new Dictionary<string, object>();
            switch (option)
            {
                case "Driver":
                    AppHelper.CurrentModule = MobileModule.Driver;
                    if (AppHelper.DriverData() != null && AppHelper.DriverData().ApprovalStatus == ApprovalStatus.Approved)
                    {
                        await _navigationService.PushAsync(ServiceHelper.GetService<DriverDashboardPage>());
                    }
                    else
                    {
                        await _navigationService.PushAsync(ServiceHelper.GetService<DriverRegistrationPage>());
                    }
                    break;

                case "Acting Driver":
                    AppHelper.CurrentModule = MobileModule.Driver;
                    if (AppHelper.DriverData() != null && AppHelper.DriverData().ApprovalStatus == ApprovalStatus.Approved)
                    {
                        await _navigationService.PushAsync(ServiceHelper.GetService<DriverDashboardPage>());
                    }
                    else
                    {
                        await _navigationService.PushAsync(ServiceHelper.GetService<DriverRegistrationPage>());
                    }
                    break;

                case "Vendor":
                    AppHelper.CurrentModule = MobileModule.Vendor;
                    await _navigationService.PushAsync(ServiceHelper.GetService<BaseProfilePage>());
                    break;

                case "ServiceProvider":
                    AppHelper.CurrentModule = MobileModule.ServiceProvider;
                    await _navigationService.PushAsync(ServiceHelper.GetService<BaseProfilePage>());
                    break;

                case "SparParts":
                    AppHelper.CurrentModule = MobileModule.SparParts;
                    await _navigationService.PushAsync(ServiceHelper.GetService<BaseProfilePage>());
                    break;

                case "BuyAndSell":
                    AppHelper.CurrentModule = MobileModule.BuyAndSell;
                    var page = ServiceHelper.GetService<RegistrationBasePage>();
                    nvparam = new Dictionary<string, object>
                    {
                        {"Tile", UserRoles.BuyAndSell }
                    };
                    await _navigationService.PushAsync(page, nvparam);
                    break;

                default:
                    // Handle unknown case
                    break;
            }
        }

        private void OpenNotification()
        {
        }

        private void OpenReferFriend()
        {
        }

        private void OpenSettings()
        {
        }

        private void ShowPayment()
        {
        }

        private void ShowRideHistory()
        {
        }

        #endregion
    }
}
