using ZCarsDriver.Core;
using ZCarsDriver.Services.Contracts;
using ZhooCars.Model.DTOs;
using ZhooCars.Model.Response;
using ZhooCars.Services;
using ZhooSoft.Core;
using ZhooSoft.ServiceBase;

namespace ZCarsDriver.Services.Services
{
    public class UserService : IUserService
    {
        #region Fields

        private readonly IZhooCarService _apiService;

        #endregion

        #region Constructors

        public UserService(IZhooCarService apiService)
        {
            _apiService = apiService;
        }

        #endregion

        #region Methods

        public async Task<ApiResponse<UserDashboardDto>> GetUserDashBoardDetailsAsync(string? phoneNumber = null)
        {
            var url = $"{ApiConstants.BaseUrl}{ApiConstants.GetUserDashBoardDetails}";
            return await _apiService.GetAsync<UserDashboardDto>(url);
        }

        public async Task<ApiResponse<UserDetailDto>> GetUserDetailsAsync(string? phoneNumber = null)
        {
            return await _apiService.GetAsync<UserDetailDto>($"{ApiConstants.BaseUrl}{ApiConstants.GetUserDetails}");
        }

        public async Task<ApiResponse<UserDetailDto>> UpsertUserDetailsAsync(UserDetailDto userDetails, string? phoneNumber = null)
        {
            var result = await _apiService.PostAsync<UserDetailDto>($"{ApiConstants.BaseUrl}{ApiConstants.UpsertUserDetails}", userDetails);

            if (!result.IsSuccess)
                return result;

            GenericPreferenceService.Instance.Set(GlobalConstants.UserDetailData, userDetails);

            switch (userDetails.Role)
            {
                case ZhooCars.Common.UserRoles.Driver:
                    GenericPreferenceService.Instance.Set(GlobalConstants.DriverData, userDetails?.DriverDetail);
                    break;

                case ZhooCars.Common.UserRoles.Vendor:
                    GenericPreferenceService.Instance.Set(GlobalConstants.VendorData, userDetails?.VendorDetail);
                    break;
            }

            return result;
        }

        #endregion
    }
}
