namespace ZhooSoft.ServiceBase
{
    #region Interfaces

    public interface IApiService
    {
        #region Methods

        Task<ApiResponse<bool>> DeleteAsync(string url);

        Task<ApiResponse<T>> GetAsync<T>(string url);

        Task<ApiResponse<T>> PostAsync<T>(string url, object data);

        Task<ApiResponse<T>> PutAsync<T>(string url, object data);

        #endregion
    }

    #endregion

}
