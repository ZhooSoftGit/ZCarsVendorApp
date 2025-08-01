using ZCars.Model.Response;
using ZhooCars.Model.DTOs;
using ZhooCars.Model.Request;
using ZhooCars.Model.Response;
using ZhooSoft.ServiceBase;

namespace ZCarsDriver.Services.Contracts
{
    #region Interfaces

    public interface IVehicleDetailsService
    {
        #region Methods

        Task<ApiResponse<VehicleDto>> GetVehicleByIdAsync(int id);

        Task<ApiResponse<PagedResponse<VehicleDto>>> GetVehiclesByFilterAsync(VehicleFilterDto filterDto);

        Task<ApiResponse<RegisterVehicleResponse>> RegisterVehicleDetailsAsync(List<DocumentUploadRequest> documents, string registerNumber, string? phoneNumber = null);

        Task<ApiResponse<bool>> UpdateInsuranceAsync(int id, InsuranceUpdateDto insuranceDto);

        Task<ApiResponse<VehicleDto>> UpsertVehicleAsync(VehicleDto vehicleDto, string? phoneNumber = null);

        Task<ApiResponse<bool>> DeleteVehicleAsync(int id);

        #endregion
    }

    #endregion

}
