using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using ZCarsDriver.Helpers;
using ZCarsDriver.UIModel;
using ZCarsDriver.Views;
using ZCarsDriver.Views.Driver;
using ZhooCars.Common;
using ZhooSoft.Core;

namespace ZCarsDriver.ViewModel
{
    public partial class DriverRegistrationViewModel : ViewModelBase
    {
        #region Fields

        [ObservableProperty]
        private ObservableCollection<CheckListItem> _checkListItems;

        #endregion

        #region Constructors

        public DriverRegistrationViewModel()
        {
            CheckListCmd = new AsyncRelayCommand<CheckListItem>(OnChecklistTapped);
        }

        #endregion

        #region Properties

        public IAsyncRelayCommand CheckListCmd { get; }

        #endregion

        #region Methods

        public override void OnAppearing()
        {
            base.OnAppearing();
            IsBusy = true;

            PageTitleName = "Driver Profile";
            LoadCheckList();
            IsBusy = false;
        }

        private void LoadCheckList()
        {
            var checklist = new List<CheckListItem>
            {
                new() { ItemName = "Vehicle Selection", IconName="\uEA04", IsCompleted = false, IsDocument = false, CheckListType = UIHelper.CheckListType.VehicleSelection},
                new() { ItemName = "Profile Information", IconName="\uE918", IsCompleted = false, IsDocument = false, CheckListType = UIHelper.CheckListType.BasicDetails},
                new () { ItemName = "Aadhaar", IconName="\uE928", IsCompleted = false, IsDocument = true, CheckListType = UIHelper.CheckListType.AadharCard, FrontDocType = DocumentTypes.AadharFront, BackDocType = DocumentTypes.AadharBack },
                new () { ItemName = "Driving License", IconName="\uE954", IsCompleted = false, IsDocument = true, CheckListType = UIHelper.CheckListType.DrivingLicense, FrontDocType = DocumentTypes.LicenseFront, BackDocType = DocumentTypes.LicenseBack }
            };

            if (AppHelper.CurrentModule == MobileModule.Driver)
            {
                checklist.Add(new CheckListItem { ItemName = "RC Book", IconName = "\uE925", IsCompleted = false, IsDocument = true, CheckListType = UIHelper.CheckListType.RCBook, FrontDocType = DocumentTypes.RcBookFront, BackDocType = DocumentTypes.RcBookBack });
            }

            var docs = AppHelper.GetDocuments();
            var driverData = AppHelper.DriverData();

            checklist[0].ApprovalStatus = driverData?.ApprovalStatus ?? (AppHelper.DriverVehicleType != null ? ApprovalStatus.Submitted : null);

            if (driverData != null)
            {
                if (driverData.VehicleType != null)
                {
                    checklist[0].ApprovalStatus = ApprovalStatus.Submitted;
                }
                checklist[1].ApprovalStatus = driverData.ApprovalStatus;
            }

            if (docs != null)
            {
                foreach (var item in checklist)
                {
                    var docStatus = docs.FirstOrDefault(x => x.DocumentTypeId == item.FrontDocType);
                    if (docStatus != null)
                    {
                        item.ApprovalStatus = docStatus.ApprovalStatus;
                    }
                }
            }

            CheckListItems = new ObservableCollection<CheckListItem>(checklist);
        }

        private async Task OnChecklistTapped(CheckListItem? item)
        {
            switch (item.CheckListType)
            {
                case UIHelper.CheckListType.VehicleSelection:
                    await _navigationService.PushAsync(ServiceHelper.GetService<VehicleTypeSelectionPage>());
                    break;
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
                case UIHelper.CheckListType.DrivingLicense:
                    param = new Dictionary<string, object>
                    {
                        {"checklist", item }
                    };
                    await _navigationService.PushAsync(ServiceHelper.GetService<DocumentUploadPage>(), param);
                    break;
                case UIHelper.CheckListType.RCBook:
                    param = new Dictionary<string, object>
                    {
                        {"checklist", item }
                    };
                    await _navigationService.PushAsync(ServiceHelper.GetService<DocumentUploadPage>(), param);
                    break;
            }
        }

        #endregion
    }
}
