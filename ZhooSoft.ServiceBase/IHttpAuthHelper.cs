namespace ZhooSoft.ServiceBase
{
    #region Interfaces

    public interface IHttpAuthHelper
    {
        #region Methods

        Task<AuthInfo> GetUserAuthInfo();

        #endregion
    }

    #endregion

}
