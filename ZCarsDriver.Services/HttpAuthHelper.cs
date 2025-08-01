using ZCarsDriver.Services.Session;
using ZhooSoft.ServiceBase;

namespace ZhooSoft.Core
{
    public class HttpAuthHelper : IHttpAuthHelper
    {
        #region Fields

        private readonly IUserSessionManager _userSessionManager;

        #endregion

        #region Constructors

        public HttpAuthHelper(IUserSessionManager userSessionManager)
        {
            _userSessionManager = userSessionManager;
        }

        #endregion

        #region Methods

        public async Task<AuthInfo> GetUserAuthInfo()
        {
            var session = await _userSessionManager.GetUserSessionAsync();

            if (session == null) return new AuthInfo();

            return new AuthInfo
            {
                RefreshToken = session.RefreshToken,
                Token = session.AccessToken,
                UserId = session.PhoneNumber
            };
        }

        #endregion
    }
}
