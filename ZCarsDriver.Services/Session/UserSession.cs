using ZhooCars.Common;

namespace ZhooSoft.Core.Session
{
    public class UserSession
    {
        #region Properties

        public string AccessToken { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string RefreshToken { get; set; }

        public List<UserRoles> Roles { get; set; }

        public string? UserId { get; set; }

        #endregion
    }
}
