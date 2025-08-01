using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using ZCarsDriver.Core;
using ZCarsDriver.Services.Contracts;
using ZCarsDriver.UIModel;
using ZCarsDriver.Views.Driver;
using ZhooCars.Model.DTOs;
using ZhooSoft.Core;

namespace ZCarsDriver.ViewModel
{
    public partial class DriverLinkDetailsViewModel : ViewModelBase
    {
        #region Fields

        [ObservableProperty]
        private bool _hasPendingVehicle;

        [ObservableProperty]
        private string _linkedVehicleInfo;

        [ObservableProperty]
        private bool _showChooseSection;

        [ObservableProperty]
        private bool _showLinkedSection;

        [ObservableProperty]
        private bool _showNewUserSection;

        [ObservableProperty]
        private bool _userHasOwnVehicles;

        #endregion

        #region Constructors

        public DriverLinkDetailsViewModel()
        {
            PageTitleName = "Driver Link Details";
            LinkOwnVehicleCommand = new Command(async () => await LinkOwnVehicle());
            LinkViaVendorCommand = new Command(async () => await LinkViaVendor());
        }

        #endregion

        #region Properties

        public ICommand LinkOwnVehicleCommand { get; }

        public ICommand LinkViaVendorCommand { get; }

        #endregion

        #region Methods

        public override async void OnAppearing()
        {
            base.OnAppearing();
            IsBusy = true;
            var linkedData = GenericPreferenceService.Instance.Get<DriverVehicleLinkDto>(GlobalConstants.VehicleLinkData);

            var vehicles = await GetVehicles();

            if (linkedData != null)
            {
                ShowLinkedSection = true;
                LinkedVehicleInfo = $"Vehicle No: {linkedData.VehicleId}\nType: {linkedData?.VehicleId}";
            }

            UserHasOwnVehicles = vehicles?.Count > 0;

            ShowChooseSection = linkedData == null && vehicles?.Count > 0;

            ShowNewUserSection = linkedData == null && (vehicles == null || vehicles.Count == 0);

            if (vehicles?.Count == 1)
            {
                HasPendingVehicle = vehicles[0].ApprovalStatus == ZhooCars.Common.ApprovalStatus.Submitted;
            }
            IsBusy = false;
        }

        private async Task<List<VehicleDto>> GetVehicles()
        {
            var service = ServiceHelper.GetService<IVendorService>();

            var result = await service.GetVehicles();

            if (result.IsSuccess && result.Data != null)
            {
                return result.Data;
            }

            return null;
        }

        private async Task LinkOwnVehicle()
        {
            var param = new Dictionary<string, object>
                    {
                        {"checklist", new CheckListItem { ItemName = "Rc Book", IsCompleted = false, IsDocument = true, CheckListType = UIHelper.CheckListType.RCBook, FrontDocType = ZhooCars.Common.DocumentTypes.RcBookFront, BackDocType = ZhooCars.Common.DocumentTypes.RcBookBack } }
                    };
            await _navigationService.PushAsync(ServiceHelper.GetService<DocumentUploadPage>(), param);
        }

        private async Task LinkViaVendor()
        {
            await _navigationService.PushAsync(ServiceHelper.GetService<LinkDriverPage>());
        }

        #endregion
    }
}
