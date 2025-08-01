using ZhooSoft.Core.Session;

namespace ZhooSoft.Auth.Model
{
    public sealed class UserDetails
    {
        #region Fields

        private static readonly Lazy<UserDetails> lazy = new Lazy<UserDetails>(() => new UserDetails());

        #endregion

        #region Constructors

        private UserDetails()
        {
        }

        #endregion

        #region Properties

        public static UserDetails Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public UserSession CurrentUser { get; private set; }

        #endregion

        #region Methods

        public void ClearUser()
        {
            CurrentUser = null;
        }

        public void SetUser(UserSession user)
        {
            CurrentUser = user;
        }

        #endregion
    }
}
