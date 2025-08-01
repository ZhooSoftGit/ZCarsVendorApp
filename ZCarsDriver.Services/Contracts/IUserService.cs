using ZhooCars.Model.DTOs;
using ZhooSoft.ServiceBase;

namespace ZCarsDriver.Services.Contracts
{
    #region Interfaces

    public interface IUserService
    {
        #region Methods

        Task<ApiResponse<UserDashboardDto>> GetUserDashBoardDetailsAsync(string? phoneNumber = null);

        Task<ApiResponse<UserDetailDto>> GetUserDetailsAsync(string? phoneNumber = null);

        Task<ApiResponse<UserDetailDto>> UpsertUserDetailsAsync(UserDetailDto userDetails, string? phoneNumber = null);

        #endregion
    }

    #endregion

}
