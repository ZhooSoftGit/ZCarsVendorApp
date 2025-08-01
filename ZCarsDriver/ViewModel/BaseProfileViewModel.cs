using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using ZCarsDriver.Core;
using ZCarsDriver.Helpers;
using ZCarsDriver.Services.Contracts;
using ZCarsDriver.UIModel;
using ZCarsDriver.Views.Driver;
using ZCarsDriver.Views.Vendor;
using ZhooCars.Common;
using ZhooCars.Model.DTOs;
using ZhooSoft.Core;

namespace ZCarsDriver.ViewModel
{
    public partial class BaseProfileViewModel : ViewModelBase
    {
        #region Fields

        private readonly IVendorService? _vendorService;


        [ObservableProperty]
        private ImageSource _bannerImg;

        private MobileModule _currentModule;

        [ObservableProperty]
        private bool _enableDashBoard;

        [ObservableProperty]
        private CheckListItem _profileCheckList;

        [ObservableProperty]
        private CheckListItem _aadharCheckList;

        [ObservableProperty]
        private CheckListItem _companyCheckList;

        [ObservableProperty]
        private bool _isVendor;

        [ObservableProperty]
        private string _vehicleNo;

        #endregion

        #region Constructors

        public BaseProfileViewModel()
        {
            ProfileTapCommand = new AsyncRelayCommand(OnProfileTapped);
            ResetDataCommand = new AsyncRelayCommand(OnResetData);
            LaunchDashBoardCommand = new AsyncRelayCommand(OnLaunchDashBoard);
            ContactSupportCommand = new AsyncRelayCommand(OnContactSupport);
            VehiclesCommand = new AsyncRelayCommand(OnVehiclesCmd);

            CheckListCmd = new AsyncRelayCommand<CheckListItem>(OnChecklistTapped);

            _vendorService = ServiceHelper.GetService<IVendorService>();
        }

        private async Task OnChecklistTapped(CheckListItem? item)
        {
            switch (item.CheckListType)
            {
                case UIHelper.CheckListType.BasicDetails:
                    await _navigationService.PushAsync(ServiceHelper.GetService<ProfileInfoPage>());
                    break;
                case UIHelper.CheckListType.AadharCard:
                    var param = new Dictionary<string, object>
                    {
                        {"checklist", item }
                    };
                    await _navigationService.PushAsync(ServiceHelper.GetService<DocumentUploadPage>(), param);
                    break;
                case UIHelper.CheckListType.GSTDocument:
                    param = new Dictionary<string, object>
                    {
                        {"checklist", item }
                    };
                    await _navigationService.PushAsync(ServiceHelper.GetService<DocumentUploadPage>(), param);
                    break;
            }
        }

        #endregion

        #region Properties

        public IAsyncRelayCommand ApplicationTapCommand { get; }

        public IAsyncRelayCommand ContactSupportCommand { get; }

        public IAsyncRelayCommand DriverLinkCommand { get; }

        public IAsyncRelayCommand LaunchDashBoardCommand { get; }

        public ICommand LogoutCommand { get; }

        public IAsyncRelayCommand ProfileTapCommand { get; }

        public IAsyncRelayCommand ResetDataCommand { get; }

        public IAsyncRelayCommand VehiclesCommand { get; }

        public IAsyncRelayCommand CheckListCmd { get; }

        #endregion

        #region Methods

        public override async void OnAppearing()
        {
            base.OnAppearing();

            if (AppHelper.CurrentModule is MobileModule module)
            {
                _currentModule = module;

                PageTitleName = module.ToString() + " Registration";

                if (_currentModule == MobileModule.Vendor)
                {
                    BannerImg = "vendor_reg_bg.png";

                    await LoadVendorProfile();
                }
            }
        }

        private async Task LoadVendorProfile()
        {
            IsBusy = true;
            IsVendor = true;
            var docs = AppHelper.GetDocuments();

            var profile = new CheckListItem() { ItemName = "Profile Information", IconName = "\uE918", IsCompleted = false, IsDocument = false, CheckListType = UIHelper.CheckListType.BasicDetails };
            var aadhar = new CheckListItem() { ItemName = "Aadhaar", IconName = "\uE928", IsCompleted = false, IsDocument = true, CheckListType = UIHelper.CheckListType.AadharCard, FrontDocType = DocumentTypes.AadharFront, BackDocType = DocumentTypes.AadharBack };
            var company = new CheckListItem() { ItemName = "Company Docs (Optional)", IconName = "\uE9C9", IsCompleted = false, IsDocument = true, CheckListType = UIHelper.CheckListType.GSTDocument, FrontDocType = DocumentTypes.Gstin };

            var result = await _vendorService.GetVendorByIdAsync();

            if (result.IsSuccess)
            {
                if (result.Data != null)
                {
                    GenericPreferenceService.Instance.Set(GlobalConstants.VendorData, result.Data);
                    profile.ApprovalStatus = result.Data.ApprovalStatus;
                }
            }

            if (docs != null)
            {
                var docStatus = docs.FirstOrDefault(x => x.DocumentTypeId == DocumentTypes.AadharFront);
                if (docStatus != null)
                {
                    aadhar.ApprovalStatus = docStatus.ApprovalStatus;
                }
                docStatus = docs.FirstOrDefault(x => x.DocumentTypeId == DocumentTypes.Gstin);
                if (docStatus != null)
                {
                    company.ApprovalStatus = docStatus.ApprovalStatus;
                }
            }

            AadharCheckList = aadhar;
            CompanyCheckList = company;
            ProfileCheckList = profile;

            IsBusy = false;
        }

        private async Task OnContactSupport()
        {
            // Contact Support Logic
            Console.WriteLine("Contacting Support");
        }

        private async Task OnLaunchDashBoard()
        {
            IsBusy = true;
            if (AppHelper.CurrentModule == MobileModule.Vendor)
            {
                //await _navigationService.PopToRootAsync();
                await _navigationService.PushAsync(ServiceHelper.GetService<DashboardPage>());
            }

            IsBusy = false;
        }

        private async Task OnProfileTapped()
        {
            await _navigationService.PushAsync(ServiceHelper.GetService<ProfileInfoPage>());
        }

        private async Task OnResetData()
        {
            await _alertService.ShowAlert("info", "your data will reset", "Ok");
        }

        private async Task OnVehiclesCmd()
        {
            var nvparam = new Dictionary<string, object>
                    {
                        {"showregistrationoption",true }
                    };
            await _navigationService.PushAsync(ServiceHelper.GetService<VehicleListPage>(), nvparam);
        }

        #endregion
    }
}
