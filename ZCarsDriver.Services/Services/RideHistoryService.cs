using ZCarsDriver.Services.Contracts;
using ZhooCars.Model.DTOs;
using ZhooCars.Services;
using ZhooSoft.ServiceBase;

namespace ZCarsDriver.Services.Services
{
    public class RideHistoryService : IRideHistoryService
    {
        #region Fields

        private readonly IZhooCarService _apiService;

        #endregion

        #region Constructors

        public RideHistoryService(IZhooCarService apiService)
        {
            _apiService = apiService;
        }

        #endregion

        #region Methods

        public async Task<ApiResponse<IEnumerable<DriverRideHistoryDto>>> GetDriverRideHistoryAsync(int userId, RideHistoryFilterDto filterDto)
        {
            return await _apiService.PostAsync<IEnumerable<DriverRideHistoryDto>>(
                $"{ApiConstants.BaseUrl}{ApiConstants.DriverRideHistory}?userId={userId}", filterDto);
        }

        public async Task<ApiResponse<IEnumerable<PassengerRideHistoryDto>>> GetPassengerRideHistoryAsync(int userId, RideHistoryFilterDto filterDto)
        {
            return await _apiService.PostAsync<IEnumerable<PassengerRideHistoryDto>>(
                $"{ApiConstants.BaseUrl}{ApiConstants.PassengerRideHistory}?userId={userId}", filterDto);
        }

        #endregion
    }
}
