namespace ZhooSoft.ServiceBase
{
    public class ApiResponse<T>
    {
        #region Properties

        public T? Data { get; set; }

        public bool IsSuccess { get; set; }

        public string Message { get; set; } = string.Empty;

        #endregion
    }

}
