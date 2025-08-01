using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ZCarsDriver.Helpers;
using ZCarsDriver.Services.Contracts;
using ZhooCars.Common;
using ZhooCars.Model.Request;
using ZhooSoft.Core;

namespace ZCarsDriver.ViewModel
{
    public partial class ProfileInfoViewModel : ViewModelBase
    {
        #region Fields

        private readonly IDocumentService _documentService;

        private readonly IUserService _userService;

        private string? _newDocUrl;

        private FileResult? _profilePhoto;

        [ObservableProperty]
        private string _submitBtnTxt;

        private UserDetailDto _userDetail;

        [ObservableProperty]
        private DateTime dateOfBirth = DateTime.Today;

        [ObservableProperty]
        private IEnumerable<string> genderOptions;

        [ObservableProperty]
        private bool isEditable;

        [ObservableProperty]
        private ImageSource? profilePhoto = "profile_menu.png";

        [ObservableProperty]
        private string? selectedGender;

        [ObservableProperty]
        private string? username;

        [ObservableProperty]
        private ObservableCollection<string> _languageOption;

        [ObservableProperty]
        private string? _selectedLanguage;

        #endregion

        #region Constructors

        public ProfileInfoViewModel()
        {
            PageTitleName = "Profile Info";

            _documentService = ServiceHelper.GetService<IDocumentService>();
            _userService = ServiceHelper.GetService<IUserService>();

            GenderOptions = Enum.GetNames(typeof(Gender));

            UploadPhotoCommand = new Command(async () => await UploadPhotoAsync());
            TakePhotoCommand = new Command(async () => await TakePhotoAsync());
            RemovePhotoCommand = new Command(RemovePhoto);
            SaveCommand = new Command(async () => await SaveProfile());
        }

        #endregion

        #region Properties

        public ICommand RemovePhotoCommand { get; }

        public ICommand SaveCommand { get; }

        public ICommand TakePhotoCommand { get; }

        public ICommand UploadPhotoCommand { get; }

        #endregion

        #region Methods

        public override void OnAppearing()
        {
            base.OnAppearing();
            IsBusy = true;

            _userDetail = AppHelper.GetUserData();
            var docs = AppHelper.GetDocuments();

            var profilePhoto = docs?.FirstOrDefault(x => x.DocumentTypeId == DocumentTypes.ProfilePhoto);

            if (_userDetail != null)
            {
                Username = _userDetail.FirstName;
                DateOfBirth = _userDetail.DateOfBirth?.ToDateTime(TimeOnly.MinValue) ?? DateTime.Today;
                SelectedGender = _userDetail.Gender;
                ProfilePhoto = AppHelper.GetDocumentSource(profilePhoto?.DocumentUrl);
            }

            if (AppHelper.CurrentModule == MobileModule.Driver)
            {
                var driverData = AppHelper.DriverData();

                var LanguageOptions = new List<string>
                                                {
                                                    "English",       // Widely understood across India
                                                    "தமிழ்",        // Tamil - Tamil Nadu
                                                    "తెలుగు",       // Telugu - Andhra Pradesh & Telangana
                                                    "हिन्दी",       // Hindi - Delhi, North India, general default
                                                    "मराठी",        // Marathi - Mumbai, Maharashtra
                                                    "اردو",         // Urdu - spoken in parts of Mumbai, Delhi, Hyderabad
                                                    "বাংলা",        // Bengali - High population in cities like Delhi & Mumbai
                                                    "ગુજરાતી",      // Gujarati - Large community in Mumbai
                                                    "ಕನ್ನಡ",        // Kannada - Some presence in Bangalore and South India
                                                    "മലയാളം"       // Malayalam - Migrants in cities like Chennai, Mumbai
                                                };

                LanguageOption = new ObservableCollection<string>(LanguageOptions);
                if (driverData != null)
                {
                    SelectedLanguage = driverData.KnownLanguage;
                }
            }

            ValidateEditAccess();
            IsBusy = false;
        }

        private void RemovePhoto() => ProfilePhoto = null;

        private async Task SaveProfile()
        {
            if (!await ValidateBeforeSave()) return;

            IsBusy = true;

            if (_profilePhoto != null)
            {
                var docRequest = new DocumentUploadRequest
                {
                    UploadDocumentRequest = new UploadDocumentRequest
                    {
                        ContentType = _profilePhoto.ContentType,
                        DocumentType = DocumentTypes.ProfilePhoto,
                        HttpMethod = DocumentHttpMethod.PUT,
                        FileName = _profilePhoto.FileName
                    },
                    DocStream = await UIHelper.UIHelper.GetStreamFromResult(_profilePhoto)
                };

                var uploadResult = await _documentService.UploadDocument(docRequest);

                _newDocUrl = uploadResult?.Data?.DocumentUrl;
                await UpdateUserProfile(_newDocUrl);
            }
            else
            {
                await UpdateUserProfile(_userDetail?.ProfilePictureUrl);
            }

            IsBusy = false;
        }

        private async Task TakePhotoAsync()
        {
            try
            {
                _profilePhoto = await MediaPicker.CapturePhotoAsync();
                if (_profilePhoto != null)
                {
                    ProfilePhoto = await UIHelper.UIHelper.GetImageSource(_profilePhoto);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error capturing photo: {ex.Message}");
            }
        }

        private async Task UpdateUserProfile(string? profileUrl)
        {
            var updateResponse = await _userService.UpsertUserDetailsAsync(GetUserDetails(profileUrl));

            if (updateResponse.IsSuccess)
            {
                await _navigationService.PopAsync();
            }
            else
            {
                await _alertService.ShowAlert("Error", "Failed to update profile. Please try again.", "OK");
            }
        }

        private UserDetailDto GetUserDetails(string? profileUrl)
        {
            var userdetail = new UserDetailDto
            {
                FirstName = Username,
                Gender = SelectedGender,
                ProfilePictureUrl = profileUrl,
                Role = AppHelper.GetRoleByModule(AppHelper.CurrentModule) ?? UserRoles.User
            };

            if (AppHelper.CurrentModule == MobileModule.Driver)
            {
                userdetail.DriverDetail = new ZhooCars.Model.DTOs.DriverDetailDto { VehicleType = AppHelper.DriverVehicleType, KnownLanguage = SelectedLanguage, ApprovalStatus = ApprovalStatus.Submitted };

            }

            if (AppHelper.CurrentModule == MobileModule.Vendor)
            {
                userdetail.VendorDetail = new ZhooCars.Model.DTOs.VendorDetailDto { CompanyName = "", IsManualVerification = true, ApprovalStatus = ApprovalStatus.Submitted };

            }

            return userdetail;
        }

        private async Task UploadPhotoAsync()
        {
            try
            {
                _profilePhoto = await FilePicker.PickAsync(new PickOptions { FileTypes = FilePickerFileType.Images });
                if (_profilePhoto != null)
                {
                    ProfilePhoto = await UIHelper.UIHelper.GetImageSource(_profilePhoto) ?? ProfilePhoto;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error picking photo: {ex.Message}");
            }
        }

        private async Task<bool> ValidateBeforeSave()
        {
            if (ProfilePhoto == null)
            {
                await App.Current.MainPage.DisplayAlert("Validation", "Please upload or capture a photo before saving.", "OK");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Username))
            {
                await App.Current.MainPage.DisplayAlert("Validation", "Username is required.", "OK");
                return false;
            }

            return true;
        }

        private void ValidateEditAccess()
        {
            if (AppHelper.CurrentModule == MobileModule.Driver)
            {
                var driverData = AppHelper.DriverData();
                IsEditable = !(driverData?.ApprovalStatus == ApprovalStatus.Submitted || driverData?.ApprovalStatus == ApprovalStatus.UnderReview || driverData?.ApprovalStatus == ApprovalStatus.Approved);
                IsEditable = true;
                SubmitBtnTxt = "Register as Driver";
            }
            else if (AppHelper.CurrentModule == MobileModule.Vendor)
            {
                var vendorData = AppHelper.VendorData();
                IsEditable = vendorData?.ApprovalStatus == ApprovalStatus.Rejected || vendorData == null;
                SubmitBtnTxt = "Register as Vendor";
            }
        }

        #endregion
    }
}
