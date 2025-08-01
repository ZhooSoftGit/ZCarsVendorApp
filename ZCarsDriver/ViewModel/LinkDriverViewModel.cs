using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ZCarsDriver.Core;
using ZCarsDriver.Services.Contracts;
using ZhooCars.Model.DTOs;
using ZhooSoft.Core;

namespace ZCarsDriver.ViewModel
{
    public partial class LinkDriverViewModel : ViewModelBase
    {
        #region Fields

        [ObservableProperty]
        private bool _canContinue;

        [ObservableProperty]
        private string _enteredOtp;

        [ObservableProperty]
        private VehicleDto _selectedCab;

        [ObservableProperty]
        private bool _showOTP;

        [ObservableProperty]
        private bool _showSearchResult;

        private IDriverVehicleLinkService? _vehicleLinkService;

        [ObservableProperty]
        private string _vendorNumber;

        private List<VehicleDto> AvailableCabs;

        [ObservableProperty]
        private ObservableCollection<VehicleDto> filteredCabs;

        [ObservableProperty]
        private string searchText;

        #endregion

        #region Constructors

        public LinkDriverViewModel()
        {
            SelectCabCommand = new RelayCommand<VehicleDto>(SelectCab);
            ContinueCommand = new AsyncRelayCommand(OnContinue);
            OnSearchCabsCmd = new AsyncRelayCommand(OnSearchCabs);
            SubmitOtpCommand = new AsyncRelayCommand(OnLinkVehicle);
            SearchCommand = new RelayCommand(FilterCabs);
            OnCloseOTPCmd = new RelayCommand(OnCloseOTP);
            _vehicleLinkService = ServiceHelper.GetService<IDriverVehicleLinkService>();
            PageTitleName = "Link Vehicle";
        }

        #endregion

        #region Properties

        public IAsyncRelayCommand ContinueCommand { get; }

        public ICommand OnCloseOTPCmd { get; }

        public IAsyncRelayCommand OnSearchCabsCmd { get; }

        public ICommand SearchCommand { get; set; }

        public ICommand SelectCabCommand { get; }

        public ICommand SubmitOtpCommand { get; }

        #endregion

        #region Methods

        public override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void FilterCabs()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredCabs = new ObservableCollection<VehicleDto>(AvailableCabs);
            }
            else
            {
                var filtered = AvailableCabs.Where(c => c.VehicleRegistrationNumber.Contains(SearchText) ||
                                               c.VehicleModel.Contains(SearchText)).ToList();
                FilteredCabs = new ObservableCollection<VehicleDto>(filtered);
            }
            OnPropertyChanged(nameof(FilteredCabs));
        }

        private void OnCloseOTP()
        {
            ShowOTP = false;
        }

        private async Task OnContinue()
        {
            if (SelectedCab != null)
            {
                IsBusy = true;
                var result = await _vehicleLinkService.RequestVehicleLinkAsync(new ZhooCars.Model.DTOs.RequestVehicleLinkDto { VendorPhoneNumber = VendorNumber, VehicleId = SelectedCab.VehicleId.Value });

                IsBusy = false;
                if (result.IsSuccess)
                {
                    ShowOTP = true;
                }
            }
        }

        private async Task OnLinkVehicle()
        {
            IsBusy = true;
            var result = await _vehicleLinkService.VerifyVehicleLinkAsync(new ZhooCars.Model.DTOs.VerifyVehicleLinkDto { OTP = EnteredOtp, VendorPhoneNumber = VendorNumber });

            if (result.IsSuccess)
            {
                ShowOTP = false;
                GenericPreferenceService.Instance.Set(GlobalConstants.VehicleLinkData, result.Data);
                await _navigationService.PopAsync();
            }
            else
            {
                await _alertService.ShowAlert("error", "Invalid OTP", "Okay");
            }

            IsBusy = false;
        }

        private async Task OnSearchCabs()
        {
            IsBusy = true;
            var result = await _vehicleLinkService.GetVehiclesByVendorPhoneNumberAsync(VendorNumber);
            if (result.IsSuccess)
            {
                AvailableCabs = result.Data;
                FilteredCabs = new ObservableCollection<VehicleDto>(result.Data);
            }
            ShowSearchResult = true;
            IsBusy = false;
        }

        private void SelectCab(VehicleDto cab)
        {
            if (cab == null) return;

            foreach (var c in FilteredCabs)
                c.IsSelected = false;

            SelectedCab = cab;

            SelectedCab.IsSelected = true;

            CanContinue = true;
        }

        #endregion
    }
}
