using ZCars.Model.Response;
using ZCarsDriver.Services;
using ZhooCars.Model;
using ZhooCars.Model.Response;
using ZhooSoft.ServiceBase;

namespace ZhooCars.Services
{
    public class AccountService : IAccountService
    {
        #region Fields

        private readonly IZhooCarService _apiService;

        private readonly IHttpAuthHelper _authHelper;

        private readonly HttpClient _httpClient;

        #endregion

        #region Constructors

        public AccountService(IZhooCarService apiService, HttpClient httpClient, IHttpAuthHelper authHelper)
        {
            _apiService = apiService;
            _httpClient = httpClient;
            _authHelper = authHelper;
        }

        #endregion

        #region Methods

        public async Task<ApiResponse<TokenResponse?>> RefreshTokenAsync(string refreshToken)
        {
            return await _apiService.PostAsync<TokenResponse?>($"{ApiConstants.BaseUrl}{ApiConstants.AccountRefreshToken}", new RefreshTokenDto { RefreshToken = refreshToken, UserId = 0 });
        }

        public async Task<ApiResponse<bool>> ReSendOtpAsync(string phoneNumber)
        {
            return await _apiService.PostAsync<bool>($"{ApiConstants.BaseUrl}{ApiConstants.AccountReSendOtp}", new { PhoneNumber = phoneNumber });
        }

        public async Task<ApiResponse<bool>> SendOtpAsync(string phoneNumber)
        {
            return await _apiService.PostAsync<bool>($"{ApiConstants.BaseUrl}{ApiConstants.AccountSendOtp}", new { PhoneNumber = phoneNumber });
        }

        public async Task<ApiResponse<OTPResponse?>> VerifyOtpAsync(string phoneNumber, string otpCode)
        {
            return await _apiService.PostAsync<OTPResponse?>($"{ApiConstants.BaseUrl}{ApiConstants.AccountVerifyOtp}", new { PhoneNumber = phoneNumber, Code = otpCode });
        }

        #endregion
    }
}
