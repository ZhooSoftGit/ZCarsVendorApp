using ZhooSoft.ServiceBase;

namespace ZhooCars.Services
{
    public interface IZhooCarService
    {
        #region Methods

        Task<ApiResponse<bool>> DeleteAsync(string url);

        Task<ApiResponse<T>> GetAsync<T>(string url);

        Task<ApiResponse<T>> PostAsync<T>(string url, object data);

        Task<ApiResponse<T>> PutAsync<T>(string url, object data);

        #endregion
    }
}
