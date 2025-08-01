﻿using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Toolkit.Hosting;
using ZCarsDriver.CoreHelper;
using ZCarsDriver.Services.Contracts;
using ZCarsDriver.Services.Services;
using ZCarsDriver.Services.Session;
using ZCarsDriver.ViewModel;
using ZCarsDriver.Views;
using ZCarsDriver.Views.Driver;
using ZhooCars.Services;
using ZhooSoft.Auth;
using ZhooSoft.Auth.ViewModel;
using ZhooSoft.Auth.Views;
using ZhooSoft.Controls;
using ZhooSoft.Core;
using ZhooSoft.Core.Alerts;
using ZhooSoft.Core.NavigationBase;
using ZhooSoft.ServiceBase;
using CommunityToolkit.Maui.Maps;
using ZCarsDriver.DPopup;
using ZCarsDriver.NavigationExtension;
using ZCarsDriver.Services.AppService;
using ZCarsDriver.Services;
using ZCarsDriver.Views.Common;
using ZCarsDriver.Views.Rides;
using ZCarsDriver.Views.Vendor;




#if ANDROID
using ZCarsDriver.Platforms;
using ZCarsDriver.PlatformHelper;
using ZCarsDriver.Platforms.Droid.Helpers;
using ZhooSoft.Controls.Platforms.Droid;
using Android.Content.Res;
#endif

namespace ZCarsDriver
{
    public static class MauiProgram
    {
        public static string GoogleMapsKey = "AIzaSyDz-0CVEdMMiWHfuXm_zXZp2t69963hCkY";

        #region Methods

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureSyncfusionToolkit()
                .UseMauiCommunityToolkit()
                .ConfigureMauiHandlers(handlers =>
                {
                    handlers.AddHandler(typeof(CustomEntry), typeof(CustomEntryHandler));
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("zapp.ttf", "zappfont");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddServices();
            builder.Services.AddViewModel();
            builder.Services.AddPages();
            builder.Services.AddStoreManager();

            builder.Services.AddSingleton<IMainAppNavigation, MainAppNavigation>();

            builder.UseMauiApp<App>().UseMauiMaps()
               .UseMauiCommunityToolkit()
               .UseMauiCommunityToolkitMaps(GoogleMapsKey);

#if ANDROID
            DependencyService.Register<IOverlayService, OverlayService>();
            builder.ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler<CustomMapView, CustomMapHandler>();
            });

#endif

            var app = builder.Build();

            ServiceHelper.Initialize(app.Services);

            AddCustomUI();

            return app;
        }

        private static void AddCustomUI()
        {
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
            {
#if ANDROID
            handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#endif
            });

            Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping("EditorCustomization", (handler, view) =>
            {
#if ANDROID
        handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#endif
            });
            Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping("PickerCustomization", (handler, view) =>
            {
#if ANDROID
        handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#endif
            });
            Microsoft.Maui.Handlers.DatePickerHandler.Mapper.AppendToMapping("DatePickerCustomization", (handler, view) =>
            {
#if ANDROID
        handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#endif
            });
        }

        private static IServiceCollection AddStoreManager(this IServiceCollection services)
        {
           

            return services;
        }

        private static IServiceCollection AddPages(this IServiceCollection services)
        {
            services.AddTransient<SplashScreen>();

            services.AddTransient<LoginPage>();
            services.AddTransient<OTPVerificationPage>();
            services.AddTransient<HomeViewPage>();
            services.AddTransient<RegistrationBasePage>();
            services.AddTransient<DocumentUploadPage>();
            services.AddTransient<DriverDashboardPage>();
            services.AddTransient<BaseProfilePage>();
            services.AddTransient<ProfileInfoPage>();

            services.AddTransient<VendorOtpPage>();
            services.AddTransient<LinkDriverPage>();
            services.AddTransient<OtpVerificationPage>();

            services.AddTransient<VehicleListPage>();

            services.AddTransient<CancelTripPage>();
            services.AddTransient<TripDetailsPage>();

            services.AddTransient<CustomMapWebView>();
            services.AddTransient<BookingRequestPopup>();

            services.AddTransient<CommonFormPage>();

            services.AddTransient<RideDetailsPage>();
            services.AddTransient<RideListPage>();
            services.AddTransient<DashboardPage>();
            services.AddTransient<VehicleDetailsPage>();

            services.AddTransient<EarningsPage>();
            services.AddTransient<PeakHoursPage>();

            services.AddTransient<DriverLinkDetailsPage>();

            services.AddTransient<SliderMenuPage>();
            services.AddTransient<DriverRegistrationPage>();
            services.AddTransient<VehicleTypeSelectionPage>();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IAlertService, AlertService>();
            services.AddSingleton<IAppNavigation, AppNavigation>();
            services.AddSingleton<INetworkService, NetworkService>();

#if ANDROID
            services.AddSingleton<IProgressService, ProgressService_Android>();
            services.AddTransient<ILocationHelper, LocationHelperAndroid>();
#endif

#if IOS
            services.AddSingleton<IProgressService, ProgressService_iOS>();
#endif
            services.AddSingleton<IUserSessionManager, UserSessionManager>();
            services.AddSingleton<HttpClient>(sp =>
            {
                return new HttpClient
                {
                    Timeout = TimeSpan.FromSeconds(10) // ⏱️ Set max timeout
                };
            });
            services.AddSingleton<IHttpAuthHelper, HttpAuthHelper>();

            services.AddSingleton<IApiService, ApiService>();
            services.AddSingleton<IZhooCarService, ZhooCarService>();

            services.AddSingleton<IAccountService, AccountService>();
            services.AddSingleton<IDocumentService, DocumentService>();
            services.AddSingleton<IDriverService, DriverService>();
            services.AddSingleton<IDriverShiftLogService, DriverShiftLogService>();
            services.AddSingleton<IDriverVehicleLinkService, DriverVehicleLinkService>();
            services.AddSingleton<IPeakHoursService, PeakHoursService>();
            services.AddSingleton<IRideHistoryService, RideHistoryService>();
            services.AddSingleton<IRideTripService, RideTripService>();
            services.AddSingleton<IServiceProviderService, ServiceProviderService>();
            services.AddSingleton<IServiceRequestService, ServiceRequestService>();
            services.AddSingleton<ISparePartsProviderService, SparePartsProviderService>();
            services.AddSingleton<ITaxiBookingService, TaxiBookingService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IVehicleDetailsService, VehicleDetailsService>();
            services.AddSingleton<IVehicleLocationService, VehicleLocationService>();
            services.AddSingleton<IVehicleMetadataService, VehicleMetadataService>();
            services.AddSingleton<IVendorService, VendorService>();

            #region
            services.AddSingleton<IOsrmService, OsrmService>();
            services.AddSingleton<ICallService, CallService>();
            services.AddSingleton<DriverSignalRService>();
            #endregion

            return services;
        }

        private static IServiceCollection AddViewModel(this IServiceCollection services)
        {
            services.AddTransient<LoginViewModel>();
            services.AddTransient<OTPVerificationViewModel>();
            services.AddTransient<HomeViewModel>();
            services.AddTransient<RegistrationBaseViewModel>();
            services.AddTransient<DocumentUploadViewModel>();
            services.AddTransient<DynamicFormViewModel>();

            services.AddTransient<BaseProfileViewModel>();
            services.AddTransient<ProfileInfoViewModel>();

            services.AddTransient<CancelTripViewModel>();
            services.AddTransient<TripDetailsViewModel>();

            services.AddTransient<VehicleListViewModel>();

            services.AddTransient<DriverDashboardViewModel>();
            services.AddTransient<CustomMapWebViewModel>();

            services.AddTransient<LinkDriverViewModel>();
            services.AddTransient<VendorOtpViewModel>();
            services.AddTransient<OtpVerificationViewModel>();


            services.AddTransient<DashboardViewModel>();
            services.AddTransient<VehicleDetailsViewModel>();
            services.AddTransient<RideListViewModel>();
            services.AddTransient<RideDetailsViewModel>();

            services.AddTransient<EarningsViewModel>();
            services.AddTransient<PeakHoursViewModel>();

            services.AddTransient<DriverLinkDetailsViewModel>();

            services.AddTransient<SliderMenuViewModel>();

            services.AddTransient<DriverRegistrationViewModel>();
            services.AddTransient<VehicleTypeSelectionViewModel>();
            return services;
        }

        #endregion
    }
}
