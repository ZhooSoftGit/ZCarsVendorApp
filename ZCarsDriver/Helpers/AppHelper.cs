using ZCars.Model.Common;
using ZCarsDriver.Core;
using ZCarsDriver.Core.Stoage;
using ZCarsDriver.Services.Session;
using ZCarsDriver.UIModel;
using ZhooCars.Common;
using ZhooCars.Model.DTOs;
using ZhooSoft.Auth;
using ZhooSoft.Core;

namespace ZCarsDriver.Helpers
{
    public static class AppHelper
    {
        public static CurrentRide? CurrentRide
        {
            get
            {
                if (_currentRide == null)
                {
                    _currentRide = RideStorageService.Load();
                }

                return _currentRide;
            }
            set
            {
                if (value == null)
                {
                    RideStorageService.Clear();
                }
                _currentRide = value;
            }
        }

        public static bool InitialLoadDone { get; set; }
        public static VehicleTypeEnum? DriverVehicleType { get; internal set; }

        public static MobileModule CurrentModule;
        private static Location? currentLocation;
        private static CurrentRide? _currentRide;

        public static async Task<Location> GetUserLocation()
        {
            try
            {
                currentLocation = await Geolocation.GetLocationAsync() ?? await Geolocation.GetLastKnownLocationAsync();
                return currentLocation;
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }

        public static List<DocumentDto> GetDocuments()
        {
            return GenericPreferenceService.Instance.Get<List<DocumentDto>>(GlobalConstants.UserDocs);
        }

        public static UserDetailDto GetUserData()
        {
            return GenericPreferenceService.Instance.Get<UserDetailDto>(GlobalConstants.UserDetailData);
        }

        public static DriverDetailDto DriverData()
        {
            return GenericPreferenceService.Instance.Get<DriverDetailDto>(GlobalConstants.DriverData);
        }

        public static DriverDetailDto VendorData()
        {
            return GenericPreferenceService.Instance.Get<DriverDetailDto>(GlobalConstants.VendorData);
        }

        public static DriverVehicleLinkDto DriverLinkData()
        {
            return GenericPreferenceService.Instance.Get<DriverVehicleLinkDto>(GlobalConstants.VehicleLinkData);
        }

        public static UserRoles? GetRoleByModule(MobileModule module)
        {
            return module switch
            {
                MobileModule.Driver => UserRoles.Driver,
                MobileModule.Vendor => UserRoles.Vendor,
                MobileModule.ServiceProvider => UserRoles.ServiceProvider,
                MobileModule.SparParts => UserRoles.SparPartsDistributor,
                MobileModule.BuyAndSell => UserRoles.BuyAndSell,

                _ => null // default fallback
            };
        }

        public static void OnLogout()
        {
            InitialLoadDone = false;
            ServiceHelper.GetService<IUserSessionManager>().ClearSession();
            GenericPreferenceService.Instance.ClearAll();
            ServiceHelper.GetService<IMainAppNavigation>().OnLogout();            
        }

        public static ImageSource? GetDocumentSource(string? documentUrl)
        {
            var result = Uri.TryCreate(documentUrl, UriKind.Absolute, out var uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (result && documentUrl != null) return ImageSource.FromUri(new Uri(documentUrl));

            return null;
        }

        public static class ApprovalStatusStyle
        {
            public static (Color BackgroundColor, Color TextColor, string LabelText) GetStyle(ApprovalStatus status)
            {
                return status switch
                {
                    ApprovalStatus.NotUploaded => (Color.FromArgb("#FFF5D7"), Color.FromArgb("#996600"), "No Document"),
                    ApprovalStatus.NotSubmitted => (Color.FromArgb("#FFE5B4"), Color.FromArgb("#A65C00"), "Not Submitted"),
                    ApprovalStatus.Submitted => (Color.FromArgb("#FDE2E2"), Color.FromArgb("#C0392B"), "Submitted"),
                    ApprovalStatus.UnderReview => (Color.FromArgb("#D6EAF8"), Color.FromArgb("#21618C"), "Under Review"),
                    ApprovalStatus.Approved => (Color.FromArgb("#D4EDDA"), Color.FromArgb("#155724"), "Approved"),
                    ApprovalStatus.Rejected => (Color.FromArgb("#F8D7DA"), Color.FromArgb("#721C24"), "Rejected"),
                    ApprovalStatus.Resubmitted => (Color.FromArgb("#FFF3CD"), Color.FromArgb("#856404"), "Resubmitted"),
                    ApprovalStatus.Expired => (Color.FromArgb("#E2E3E5"), Color.FromArgb("#6C757D"), "Expired"),
                    _ => (Colors.LightGray, Colors.Black, "Unknown")
                };
            }
        }
    }
}
