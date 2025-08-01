using ZhooSoft.Core.Session;

namespace ZCarsDriver.Services.Session
{
    #region Interfaces

    public interface IUserSessionManager
    {
        #region Methods

        void ClearSession();

        string GetUserName();

        string GetUserPhoneNumber();

        Task<string?> GetUserPreference(string key);

        Task<UserSession?> GetUserSessionAsync();

        Task SaveUserSessionAsync(UserSession session);

        Task SetUserPreference(string key, string value);

        #endregion
    }

    #endregion
}
