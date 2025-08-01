namespace ZhooSoft.Auth
{
    #region Interfaces

    public interface IMainAppNavigation
    {
        #region Methods

        void NavigateToDetail(NavigationPage detailPage);

        void NavigateToMain(bool IsInitialLoad = false);

        void NavigateToNotification();

        void OnLogout();

        #endregion
    }

    #endregion
}
