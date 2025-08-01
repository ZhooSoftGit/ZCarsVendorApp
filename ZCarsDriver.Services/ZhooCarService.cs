
using ZhooCars.Services;
using ZhooSoft.Core.Alerts;
using ZhooSoft.ServiceBase;

namespace ZCarsDriver.Services
{
    public class ZhooCarService : IZhooCarService
    {
        private readonly IApiService _apiService;
        private readonly INetworkService _networkService;
        private readonly IAlertService _alertService;

        public ZhooCarService(IApiService apiService, INetworkService networkService, IAlertService alertService) 
        {
            _apiService = apiService;
            _networkService = networkService;
            _alertService = alertService;
        }
        public async Task<ApiResponse<T>> GetAsync<T>(string url)
        {
            if (!_networkService.IsConnected())
            {
                await _alertService.ShowAlert("Network Error", "Please check your internet connection.", "OK");
                return new ApiResponse<T> { IsSuccess = false };
            }

            try
            {
                var response = await _apiService.GetAsync<T>(url);

                if (!response.IsSuccess)
                {
                    await _alertService.ShowAlert("Error", response.Message ?? "Something went wrong.", "OK");
                }

                return response;
            }
            catch (Exception ex)
            {
                await _alertService.ShowAlert("Exception", ex.Message, "OK");
                return new ApiResponse<T> { IsSuccess = false };
            }
        }

        public async Task<ApiResponse<T>> PostAsync<T>(string url, object data)
        {
            if (!_networkService.IsConnected())
            {
                await _alertService.ShowAlert("Network Error", "Please check your internet connection.", "OK");
                return new ApiResponse<T> { IsSuccess = false };
            }

            try
            {
                var response = await _apiService.PostAsync<T>(url, data);

                if (!response.IsSuccess)
                {
                    await _alertService.ShowAlert("Error", response.Message ?? "Something went wrong.", "OK");
                }

                return response;
            }
            catch (Exception ex)
            {
                await _alertService.ShowAlert("Exception", ex.Message, "OK");
                return new ApiResponse<T> { IsSuccess = false };
            }
        }

        public async Task<ApiResponse<T>> PutAsync<T>(string url, object data)
        {
            if (!_networkService.IsConnected())
            {
                await _alertService.ShowAlert("Network Error", "Please check your internet connection.", "OK");
                return new ApiResponse<T> { IsSuccess = false };
            }

            try
            {
                var response = await _apiService.PutAsync<T>(url, data);

                if (!response.IsSuccess)
                {
                    await _alertService.ShowAlert("Error", response.Message ?? "Something went wrong.", "OK");
                }

                return response;
            }
            catch (Exception ex)
            {
                await _alertService.ShowAlert("Exception", ex.Message, "OK");
                return new ApiResponse<T> { IsSuccess = false };
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(string url)
        {
            if (!_networkService.IsConnected())
            {
                await _alertService.ShowAlert("Network Error", "Please check your internet connection.", "OK");
                return new ApiResponse<bool> { IsSuccess = false };
            }

            try
            {
                var response = await _apiService.DeleteAsync(url);

                if (!response.IsSuccess)
                {
                    await _alertService.ShowAlert("Error", response.Message ?? "Something went wrong.", "OK");
                }

                return response;
            }
            catch (Exception ex)
            {
                await _alertService.ShowAlert("Exception", ex.Message, "OK");
                return new ApiResponse<bool> { IsSuccess = false };
            }
        }
    }
}
