using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ZCarsDriver.Services.Contracts;
using ZCarsDriver.UIModel;
using ZCarsDriver.Views.Driver;
using ZCarsDriver.Views.Vendor;
using ZhooCars.Common;
using ZhooCars.Model.DTOs;
using ZhooSoft.Core;

namespace ZCarsDriver.ViewModel
{
    public partial class VehicleListViewModel : ViewModelBase
    {
        #region Fields

        [ObservableProperty]
        private ObservableCollection<VehicleDto> _filteredVehicles;

        [ObservableProperty]
        private bool _showRegistrationOption;

        private IVehicleDetailsService _vehicleDetailsService;

        [ObservableProperty]
        private ObservableCollection<VehicleDto> _vehicles;

        private IVendorService _vendorService;

        [ObservableProperty]
        private string searchText;

        #endregion

        #region Constructors

        public VehicleListViewModel()
        {
            PageTitleName = "Vehicle List";

            Vehicles = new ObservableCollection<VehicleDto>();
            FilteredVehicles = new ObservableCollection<VehicleDto>();

            SearchCommand = new RelayCommand(PerformSearch);
            EditVehicleCommand = new AsyncRelayCommand<VehicleDto>(EditVehicle);
            ViewVehicleCommand = new AsyncRelayCommand<VehicleDto>(ViewVehicle);
            DeleteVehicleCommand = new AsyncRelayCommand<VehicleDto>(DeleteVehicle);
            AddVehicleCommand = new AsyncRelayCommand(AddVehicle);

            _vehicleDetailsService = ServiceHelper.GetService<IVehicleDetailsService>();
            _vendorService = ServiceHelper.GetService<IVendorService>();
        }

        #endregion

        #region Properties

        public IAsyncRelayCommand AddVehicleCommand { get; }

        public IAsyncRelayCommand<VehicleDto> DeleteVehicleCommand { get; }

        public IAsyncRelayCommand<VehicleDto> EditVehicleCommand { get; }

        public ICommand SearchCommand { get; }

        public IAsyncRelayCommand<VehicleDto> ViewVehicleCommand { get; }

        #endregion

        #region Methods

        public override async void OnAppearing()
        {
            base.OnAppearing();

            IsBusy = true;
            if (NavigationParams.ContainsKey("showregistrationoption"))
            {
                ShowRegistrationOption = true;
            }
            await LoadData();
            PerformSearch();
            IsBusy = false;
        }

        private async Task AddVehicle()
        {
            var param = new Dictionary<string, object>
                    {
                        {"checklist", new CheckListItem { ItemName = "Rc Book", IsCompleted = false, IsDocument = true, CheckListType = UIHelper.CheckListType.RCBook } }
                    };
            await _navigationService.PushAsync(ServiceHelper.GetService<DocumentUploadPage>(), param);
        }

        private async Task DeleteVehicle(VehicleDto vehicle)
        {
            var result = await _alertService.ShowConfirmation("Info", $"Are you sure? can we delete a vehicle {vehicle.VehicleRegistrationNumber}", "ok", "cancel");

            if (result)
            {
                IsBusy = true;
                var deleteResponse = await _vehicleDetailsService.DeleteVehicleAsync(vehicle.VehicleId.Value);

                if (deleteResponse.IsSuccess)
                {
                    await _alertService.ShowAlert("Info", "Associated Vehicle is removed", "Ok");
                    await LoadData();
                }

                IsBusy = false;
            }
        }

        private async Task EditVehicle(VehicleDto vehicle)
        {
            var param = new Dictionary<string, object>
                    {
                        { "regType",  RegsitrationType.VechicleDetails }
                    };
            await _navigationService.PushAsync(ServiceHelper.GetService<RegistrationBasePage>(), param);
        }

        private async Task LoadData()
        {
            //CAL API ad get vechicle list
            IsBusy = true;

            var result = await _vendorService.GetVehicles();

            if (result.IsSuccess)
            {
                Vehicles = new ObservableCollection<VehicleDto>(result.Data);
            }

            IsBusy = false;
        }

        private void PerformSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredVehicles = new ObservableCollection<VehicleDto>(Vehicles);
            }
            else
            {
                FilteredVehicles = new ObservableCollection<VehicleDto>(Vehicles.Where(v => v.VehicleRegistrationNumber.Contains(SearchText) || v.VehicleRegistrationNumber.Contains(SearchText)));
            }
        }

        private async Task ViewVehicle(VehicleDto vehicle)
        {
            if (ShowRegistrationOption)
            {
                var param = new Dictionary<string, object>
                    {
                        { "regType",  RegsitrationType.VechicleDetails }
                    };
                await _navigationService.PushAsync(ServiceHelper.GetService<RegistrationBasePage>(), param);
            }
            else
            {
                var param = new Dictionary<string, object>
                    {
                        { "selecteItem",  vehicle }
                    };
                await _navigationService.PushAsync(ServiceHelper.GetService<VehicleDetailsPage>(), param);
            }
        }

        #endregion
    }
}
