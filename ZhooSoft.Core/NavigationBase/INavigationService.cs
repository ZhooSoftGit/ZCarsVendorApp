using CommunityToolkit.Maui.Views;

namespace ZhooSoft.Core.NavigationBase
{
    #region Interfaces

    public interface INavigationService
    {
        #region Methods

        Task ClosePopup();

        //ViewModelBase GetCurrentPageModel();
        Task ClosePopup(Dictionary<string, object> navigationParams, bool IsBackPopup = false);

        Task OnTabClicked(object obj);

        Task<object?> OpenPopup(Popup popup);

        Task<object> OpenPopup(Popup popup, Dictionary<string, object> navigationParams);

        Task PopAsync();

        Task PopAsync(Dictionary<string, object> NavigationParams);

        Task PopToRootAsync();

        Task PushAsync(Page page);

        Task PushAsync(Page page, Dictionary<string, object> navigationParams);

        Task PushModalAsync(Page page);

        #endregion
    }

    #endregion
}
