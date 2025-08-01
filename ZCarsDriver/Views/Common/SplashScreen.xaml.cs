using ZCarsDriver.Services.Session;
using ZhooCars.Common;
using ZhooCars.Services;
using ZhooSoft.Auth;
using ZhooSoft.Core;
using ZhooSoft.Core.NavigationBase;

namespace ZCarsDriver.Views.Common;

public partial class SplashScreen : ContentPage
{
    public SplashScreen()
    {
        InitializeComponent();
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        var _userSession = ServiceHelper.GetService<IUserSessionManager>();
        var session = await _userSession.GetUserSessionAsync();
        if (session != null)
        {
            _userSession.ClearSession();
            var accountService = ServiceHelper.GetService<IAccountService>();
            var result = await accountService.RefreshTokenAsync(session.RefreshToken);
            if (result.IsSuccess && result.Data != null)
            {
                session.AccessToken = result.Data.Token;
                session.RefreshToken = result.Data.RefreshToken;
                await _userSession.SaveUserSessionAsync(new ZhooSoft.Core.Session.UserSession
                {
                    AccessToken = result.Data.Token,
                    RefreshToken = result.Data.RefreshToken,
                    Name = session.Name,
                    PhoneNumber = session.PhoneNumber
                });
            }
        }
        ServiceHelper.GetService<IMainAppNavigation>().NavigateToMain(true);
    }
}