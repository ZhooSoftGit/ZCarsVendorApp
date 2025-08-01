using ZCars.Model.Response;
using ZCarsDriver.Services.Contracts;
using ZhooCars.Model.DTOs;
using ZhooCars.Model.Request;
using ZhooCars.Model.Response;
using ZhooCars.Services;
using ZhooSoft.Core;
using ZhooSoft.ServiceBase;

namespace ZCarsDriver.Services.Services
{
    public class VehicleDetailsService : IVehicleDetailsService
    {
        #region Fields

        private readonly IZhooCarService _apiService;

        #endregion

        #region Constructors

        public VehicleDetailsService(IZhooCarService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ApiResponse<bool>> DeleteVehicleAsync(int id)
        {
            return await _apiService.DeleteAsync(ApiConstants.RemoveVehicle.Replace("{id}", id.ToString()));
        }

        #endregion

        #region Methods

        public async Task<ApiResponse<VehicleDto>> GetVehicleByIdAsync(int id)
        {
            return await _apiService.GetAsync<VehicleDto>(ApiConstants.GetVehicleById.Replace("{id}", id.ToString()));
        }

        public async Task<ApiResponse<PagedResponse<VehicleDto>>> GetVehiclesByFilterAsync(VehicleFilterDto filterDto)
        {
            return await _apiService.PostAsync<PagedResponse<VehicleDto>>(ApiConstants.GetVehiclesByFilter, filterDto);
        }

        public async Task<ApiResponse<RegisterVehicleResponse>> RegisterVehicleDetailsAsync(List<DocumentUploadRequest> documents, string registerNumber, string? phoneNumber = null)
        {
            var url = $"{ApiConstants.BaseUrl}{ApiConstants.RegisterVehicleDetails}";

            var registervehicleReq = new RegisterDriverOrVehicleDto { UploadDocumentRequests = documents.Select(x => x.UploadDocumentRequest).ToList(), VehicleNumber = registerNumber };

            var result = await _apiService.PostAsync<RegisterVehicleResponse>(url, registervehicleReq);

            if (result.IsSuccess && result.Data != null && result.Data.Document != null)
            {
                ServiceHelper.GetService<IDocumentService>().UpdateLocalData(result.Data.Document);
                foreach (var item in documents)
                {
                    var outputdata = result.Data.Document?.FirstOrDefault(x => x.DocumentTypeId == item.UploadDocumentRequest.DocumentType);
                    if (outputdata != null)
                    {
                        await ServiceHelper.GetService<IDocumentService>().UploadToCDNAsync(outputdata.SignedUrlForUpload, item.DocStream, item.UploadDocumentRequest.ContentType);
                    }
                }
            }

            return result;
        }

        public async Task<ApiResponse<bool>> UpdateInsuranceAsync(int id, InsuranceUpdateDto insuranceDto)
        {
            var url = ApiConstants.UpdateInsurance.Replace("{id}", id.ToString());
            return await _apiService.PostAsync<bool>(url, insuranceDto);
        }

        public async Task<ApiResponse<VehicleDto>> UpsertVehicleAsync(VehicleDto vehicleDto, string? phoneNumber = null)
        {
            var url = string.IsNullOrEmpty(phoneNumber) ? ApiConstants.UpsertVehicle : $"{ApiConstants.UpsertVehicle}?phoneNumber={phoneNumber}";
            return await _apiService.PostAsync<VehicleDto>(url, vehicleDto);
        }

        #endregion
    }
}
