using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ZCarsDriver.Helpers;
using ZCarsDriver.UIModel;
using ZCarsDriver.Views.Driver;
using ZhooCars.Common;
using ZhooSoft.Core;

namespace ZCarsDriver.ViewModel
{
    public partial class RegistrationBaseViewModel : ViewModelBase
    {
        #region Fields

        [ObservableProperty]
        private ObservableCollection<CheckListItem> _checkListItems;

        [ObservableProperty]
        private bool _isSubmitEnabled;

        private RegsitrationType _registrationType;

        private object _selectedObj;

        private UserRoles _userRole;

        #endregion

        #region Constructors

        public RegistrationBaseViewModel()
        {
            CheckListCmd = new AsyncRelayCommand<CheckListItem>(ToggleItemStatus);
            SubmitApplicationCommand = new RelayCommand(SubmitApplication, CanSubmit);
            PageTitleName = "Registration";
        }

        #endregion

        #region Properties

        public IAsyncRelayCommand CheckListCmd { get; }

        public ICommand SubmitApplicationCommand { get; }

        #endregion

        #region Methods

        public override async void OnAppearing()
        {
            base.OnAppearing();
            IsBusy = true;
            LoadCheckList();
            await Task.Delay(100);
            IsBusy = false;
        }

        private bool CanSubmit() => IsSubmitEnabled;

        private void LoadCheckList()
        {
            if (CheckListItems == null)
            {
                var checklist = new List<CheckListItem>();

                switch (AppHelper.CurrentModule)
                {
                    case MobileModule.Driver:
                        checklist.Add(new CheckListItem { ItemName = "Aadhaar", IsCompleted = false, IsDocument = true, CheckListType = UIHelper.CheckListType.AadharCard, FrontDocType = DocumentTypes.AadharFront, BackDocType = DocumentTypes.AadharBack });
                        checklist.Add(new CheckListItem { ItemName = "Driving License", IsCompleted = false, IsDocument = true, CheckListType = UIHelper.CheckListType.DrivingLicense, FrontDocType = DocumentTypes.LicenseFront, BackDocType = DocumentTypes.LicenseBack });
                        break;

                    case MobileModule.Vendor:
                        checklist.Add(new CheckListItem { ItemName = "Aadhaar", IsCompleted = false, IsDocument = true, CheckListType = UIHelper.CheckListType.AadharCard, FrontDocType = DocumentTypes.AadharFront, BackDocType = DocumentTypes.AadharBack });
                        //checklist.Add(new CheckListItem { ItemName = "Vehicle Details", IsCompleted = false, CheckListType = UIHelper.CheckListType.VehicleDetails, IsForm = true });
                        break;

                    //case MobileModule.v:
                    //    checklist.Add(new CheckListItem { ItemName = "Vehicle Details", IsCompleted = false, CheckListType = UIHelper.CheckListType.VehicleDetails, IsForm = true });
                    //    checklist.Add(new CheckListItem { ItemName = "Rc Book", IsCompleted = false, IsDocument = true, CheckListType = UIHelper.CheckListType.RCBook, FrontDocType = DocumentTypes.RcBookFront, BackDocType = DocumentTypes.RcBookBack });
                    //    checklist.Add(new CheckListItem { ItemName = "Insurance Document", IsCompleted = false, FrontOnly = true, IsOptional = true, IsDocument = true, CheckListType = UIHelper.CheckListType.Insurance });
                    //    break;

                    case MobileModule.ServiceProvider:
                        checklist.Add(new CheckListItem { ItemName = "Service Details", IsCompleted = false, CheckListType = UIHelper.CheckListType.ServiceStationDetails, IsForm = true });
                        checklist.Add(new CheckListItem { ItemName = "GST Document", IsCompleted = false, FrontOnly = true, IsDocument = true, CheckListType = UIHelper.CheckListType.GSTDocument });
                        break;

                    case MobileModule.SparParts:
                        checklist.Add(new CheckListItem { ItemName = "Shop Details", IsCompleted = false, CheckListType = UIHelper.CheckListType.SpartPartsShopDetails, IsForm = true });
                        checklist.Add(new CheckListItem { ItemName = "GST Document", IsCompleted = false, FrontOnly = true, IsDocument = true, CheckListType = UIHelper.CheckListType.GSTDocument });
                        break;

                    default:
                        break;
                }

                UpdateCheckListData(checklist);
            }

            UpdateSubmitButtonState();
        }

        private void SubmitApplication()
        {
            _navigationService.PopAsync();
        }

        private async Task ToggleItemStatus(CheckListItem item)
        {
            if (item != null)
            {
                if (item.IsDocument)
                {
                    var param = new Dictionary<string, object>
                    {
                        {"checklist", item }
                    };
                    await _navigationService.PushAsync(ServiceHelper.GetService<DocumentUploadPage>(), param);
                }

                if (item.IsForm)
                {
                    var param = new Dictionary<string, object>
                    {
                        {"checklist", item },
                        {"data", _selectedObj }
                    };

                    await _navigationService.PushAsync(ServiceHelper.GetService<CommonFormPage>(), param);
                }

                UpdateSubmitButtonState();
            }
        }

        private void UpdateCheckListData(List<CheckListItem> checklist)
        {
            if (AppHelper.CurrentModule == MobileModule.Driver)
            {
                var docs = AppHelper.GetDocuments();
                var licenseDoc = docs?.FirstOrDefault(x => x.DocumentTypeId == DocumentTypes.LicenseFront);
                if (licenseDoc != null)
                {
                    checklist[1].ApprovalStatus = licenseDoc.ApprovalStatus;
                }
                var aasharDoc = docs?.FirstOrDefault(x => x.DocumentTypeId == DocumentTypes.AadharFront);
                if (aasharDoc != null)
                {
                    checklist[0].ApprovalStatus = aasharDoc.ApprovalStatus;
                }
            }

            CheckListItems = new ObservableCollection<CheckListItem>(checklist);
        }

        private void UpdateSubmitButtonState()
        {
            IsSubmitEnabled = CheckListItems.All(item => item.IsCompleted || item.IsOptional);
        }

        #endregion
    }
}
