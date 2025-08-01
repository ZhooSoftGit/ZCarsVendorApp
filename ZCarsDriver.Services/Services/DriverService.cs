﻿using ZCars.Model.Response;
using ZCarsDriver.Services.Contracts;
using ZhooCars.Model.DTOs;
using ZhooCars.Model.Response;
using ZhooCars.Services;
using ZhooSoft.ServiceBase;

namespace ZCarsDriver.Services.Services
{
    public class DriverService : IDriverService
    {
        #region Fields

        private readonly IZhooCarService _apiService;

        #endregion

        #region Constructors

        public DriverService(IZhooCarService apiService)
        {
            _apiService = apiService;
        }

        #endregion

        #region Methods

        public async Task<ApiResponse<DriverDetailDto>> GetDriverByIdAsync(int userId)
        {
            return await _apiService.GetAsync<DriverDetailDto>($"{ApiConstants.BaseUrl}{ApiConstants.DriverDetailGet}?userId={userId}");
        }

        public async Task<ApiResponse<DriverProfileInfo>> GetDriverProfile()
        {
            try
            {
                var result = await _apiService.GetAsync<DriverProfileInfo>($"{ApiConstants.BaseUrl}{ApiConstants.DriverProfile}");
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ApiResponse<PagedResponse<DriverListDto>>> GetDriversByFilterAsync(DriverFilterDto filter)
        {
            return await _apiService.PostAsync<PagedResponse<DriverListDto>>($"{ApiConstants.BaseUrl}{ApiConstants.DriverDetailFilter}", filter);
        }

        public async Task<ApiResponse<bool>> RegisterDriverAsync(int userId, RegisterDriverDto driverDto)
        {
            return await _apiService.PostAsync<bool>($"{ApiConstants.BaseUrl}{ApiConstants.DriverDetailRegister}?userId={userId}", driverDto);
        }

        public async Task<ApiResponse<DriverDetailDto>> UpsertDriverAsync(int userId, DriverDetailDto driverDto)
        {
            return await _apiService.PostAsync<DriverDetailDto>($"{ApiConstants.BaseUrl}{ApiConstants.DriverDetailUpdate}?userId={userId}", driverDto);
        }

        #endregion
    }
}
