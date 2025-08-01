
using ZCarsDriver.Services;
using ZCarsDriver.Views.Common;
using ZhooSoft.Core;

namespace ZCarsDriver
{
    public partial class App : Application
    {
        #region Constructors

        public App()
        {
            InitializeComponent();
            UserAppTheme = AppTheme.Light;
        }

        #endregion

        #region Methods

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(ServiceHelper.GetService<SplashScreen>());
        }

        protected override void OnResume()
        {
            base.OnResume();
            var overlayService = DependencyService.Get<IOverlayService>();
            overlayService.RemoveOverlay();
        }

        protected override void OnSleep()
        {
            base.OnSleep();
            var overlayService = DependencyService.Get<IOverlayService>();
            overlayService.ShowOverlay();
        }

    

        private void CheckLogin()
        {
        }

        #endregion
    }
}
