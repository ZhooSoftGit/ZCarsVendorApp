using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ZCarsDriver.Services.Session;
using ZhooCars.Services;
using ZhooSoft.Core;

namespace ZCarsDriver.ViewModel
{
    public partial class OtpVerificationViewModel : ViewModelBase
    {
        #region Fields

        private readonly IAccountService _accountService;

        private readonly IUserSessionManager _userSessionManager;

        [ObservableProperty]
        private bool _isResendVisible;

        [ObservableProperty]
        private bool _isTimerVisible = true;

        [ObservableProperty]
        private string _otp1;

        [ObservableProperty]
        private string _otp2;

        [ObservableProperty]
        private string _otp3;

        [ObservableProperty]
        private string _otp4;

        [ObservableProperty]
        private string _phoneNumber;

        private int _secondsRemaining;

        [ObservableProperty]
        private string _timerText;

        [ObservableProperty]
        private bool showError;

        #endregion

        #region Constructors

        public OtpVerificationViewModel()
        {
            _secondsRemaining = 60; // Set initial countdown time (1:30)
            StartTimer();
            PageTitleName = "Verify Vendor OTP";
            SubmitCommand = new AsyncRelayCommand(OnSubmit);
            ResendCodeCommand = new AsyncRelayCommand(OnResendCode);
            _accountService = ServiceHelper.GetService<IAccountService>();
            _userSessionManager = ServiceHelper.GetService<IUserSessionManager>();
        }

        #endregion

        #region Properties

        public IAsyncRelayCommand ResendCodeCommand { get; }

        public IAsyncRelayCommand SubmitCommand { get; }

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

        private async Task OnResendCode()
        {
            Otp1 = string.Empty;
            Otp2 = string.Empty;
            Otp3 = string.Empty;
            Otp4 = string.Empty;
            _secondsRemaining = -1;
            IsResendVisible = false;
            IsTimerVisible = true;
            var result = await _accountService.ReSendOtpAsync(PhoneNumber);
            if (result.IsSuccess)
            {
                await _alertService.ShowAlert("Info", "Otp has been resend successfully", "Ok");
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
            string enteredOtp = Otp1.Trim() + Otp2.Trim() + Otp3.Trim() + Otp4.Trim();

            if (enteredOtp.Length == 4)
            {
                IsBusy = true;
                var result = await _accountService.VerifyOtpAsync(PhoneNumber, enteredOtp);
                IsBusy = false;
                if (result.IsSuccess)
                {
                    await _navigationService.PopAsync();
                    await _navigationService.PopAsync();
                }
                else
                {
                    Otp1 = Otp2 = Otp3 = Otp4 = string.Empty;
                    ShowError = true;
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
