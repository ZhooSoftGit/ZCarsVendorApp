using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ZCarsDriver.Core;
using ZCarsDriver.Services.Session;
using ZhooCars.Common;
using ZhooCars.Model.DTOs;
using ZhooCars.Services;
using ZhooSoft.Core;

namespace ZhooSoft.Auth.ViewModel
{
    public partial class OTPVerificationViewModel : ViewModelBase
    {
        #region Fields

        private readonly IAccountService _accountService;

        private readonly IUserSessionManager _userSessionManager;

        [ObservableProperty]
        private bool _isResendVisible;

        [ObservableProperty]
        private bool _isTimerVisible = true;

        [ObservableProperty]
        private string _phoneNumber;

        private int _secondsRemaining;

        [ObservableProperty]
        private string _timerText;

        [ObservableProperty]
        private bool showError;

        [ObservableProperty]
        private string _enteredOtp;

        #endregion

        #region Constructors

        public OTPVerificationViewModel(IAccountService accountService, IUserSessionManager userSessionManager)
        {
            _secondsRemaining = 60; // Set initial countdown time (1:30)
            StartTimer();
            SubmitOtpCommand = new AsyncRelayCommand(OnSubmit);
            ResendCodeCommand = new AsyncRelayCommand(OnResendCode);
            ChangePhoneNumberCommand = new AsyncRelayCommand(OnChangePhoneNumber);
            _accountService = accountService;
            _userSessionManager = userSessionManager;
        }

        #endregion

        #region Properties

        public IAsyncRelayCommand ChangePhoneNumberCommand { get; }

        public IAsyncRelayCommand ResendCodeCommand { get; }

        public IAsyncRelayCommand SubmitOtpCommand { get; }

        #endregion

        #region Methods

        public override void OnAppearing()
        {
            base.OnAppearing();

            if (NavigationParams != null)
            {
                var str = NavigationParams["phoneNumber"].ToString();
                PhoneNumber = str;
            }
        }

        private List<UserRoles> GetRoles(List<UserRoleDto> roles)
        {
            List<UserRoles> roless = new List<UserRoles>();
            foreach (var item in roles)
            {
                roless.Add(item.RoleId);
            }

            return roless;
        }

        private async Task OnChangePhoneNumber()
        {
            _secondsRemaining = -1;
            await _navigationService.PopAsync();
        }

        private async Task OnResendCode()
        {
            _secondsRemaining = -1;
            IsResendVisible = false;
            IsTimerVisible = true;
            var result = await _accountService.ReSendOtpAsync(PhoneNumber);
            if (result.IsSuccess)
            {
                _secondsRemaining = 90;
                StartTimer();
            }
            else
            {
                await _alertService.ShowAlert("Error", "Otp Send is Failed", "Ok");
            }
        }

        private async Task OnSubmit()
        {
            if (EnteredOtp.Length == 4)
            {
                IsBusy = true;
                var result = await _accountService.VerifyOtpAsync(PhoneNumber, EnteredOtp);
                IsBusy = false;
                if (result.IsSuccess)
                {
                    _userSessionManager.ClearSession();
                    GenericPreferenceService.Instance.ClearAll();

                    // Create a API to get session details
                    var userSession = new Core.Session.UserSession
                    {
                        Name = result.Data?.Userdetails?.FirstName,
                        PhoneNumber = PhoneNumber,
                        RefreshToken = result.Data.TokenResponse.RefreshToken,
                        AccessToken = result.Data.TokenResponse.Token,
                        Roles = [UserRoles.User]
                    };
                    await _userSessionManager.SaveUserSessionAsync(userSession);

                    ServiceHelper.GetService<IMainAppNavigation>().NavigateToMain(true);
                }
            }
            else
            {
                ShowError = true;
            }
        }

        private async void StartTimer()
        {
            while (_secondsRemaining > 0)
            {
                TimerText = TimeSpan.FromSeconds(_secondsRemaining).ToString("mm\\:ss");
                await Task.Delay(1000);
                _secondsRemaining--;
            }
            TimerText = "";
            IsTimerVisible = false;
            IsResendVisible = true;
        }

        #endregion
    }
}
