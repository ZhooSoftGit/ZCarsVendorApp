﻿using ZCarsDriver.Services.Contracts;
using ZhooCars.Model.DTOs;
using ZhooCars.Model.Request;
using ZhooCars.Model.Response;
using ZhooCars.Services;
using ZhooSoft.ServiceBase;

namespace ZCarsDriver.Services.Services
{
    public class VendorService : IVendorService
    {
        #region Fields

        private readonly IZhooCarService _apiService;

        #endregion

        #region Constructors

        public VendorService(IZhooCarService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ApiResponse<List<VehicleDto>>> GetVehicles(string? phoneNumber = null)
        {
            var url = $"{ApiConstants.BaseUrl}{ApiConstants.GetVendorVehicle}";
            return await _apiService.GetAsync<List<VehicleDto>>(url);
        }

        #endregion

        #region Methods

        public async Task<ApiResponse<VendorDetailDto>> GetVendorByIdAsync(string? phoneNumber = null)
        {
            var url =  $"{ApiConstants.BaseUrl}{ApiConstants.GetVendorDetails}?phoneNumber={phoneNumber}";
            return await _apiService.GetAsync<VendorDetailDto>(url);
        }

        public async Task<ApiResponse<PagedResponse<VendorListDto>>> GetVendorsByFilterAsync(VendorFilterDto filter)
        {
            return await _apiService.PostAsync<PagedResponse<VendorListDto>>(ApiConstants.GetVendorsByFilter, filter);
        }

        public async Task<ApiResponse<bool>> Register(string? phoneNumber, VendorRegisterRequest vendorRegisterRequest)
        {
            var url = string.IsNullOrEmpty(phoneNumber) ? ApiConstants.VendorRegisterRequest : $"{ApiConstants.VendorRegisterRequest}?phoneNumber={phoneNumber}";
            return await _apiService.PostAsync<bool>(url, vendorRegisterRequest);
        }

        public async Task<ApiResponse<VendorDetailDto>> UpsertVendorAsync(string? phoneNumber, VendorDetailDto dto)
        {
            var url = string.IsNullOrEmpty(phoneNumber) ? ApiConstants.UpsertVendor : $"{ApiConstants.UpsertVendor}?phoneNumber={phoneNumber}";
            return await _apiService.PostAsync<VendorDetailDto>(url, dto);
        }

        #endregion
    }
}
