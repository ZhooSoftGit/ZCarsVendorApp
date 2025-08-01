using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ZCarsDriver.Core;
using ZCarsDriver.Helpers;
using ZhooCars.Common;
using ZhooCars.Model.DTOs;
using ZhooSoft.Core;

namespace ZCarsDriver.ViewModel
{
    public partial class VehicleTypeSelectionViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<VehicleCategory> _vehicleTypes;

        [ObservableProperty]
        private VehicleCategory _selectedVehicleType;

        [ObservableProperty]
        private bool _isSaveEnabled;

        public ICommand VehicleSelectCmd { get; }

        public VehicleTypeSelectionViewModel()
        {
            LoadVehicleTypes();
            ShowBackButton = false;
            PageTitleName = "Vehicle Type Selection";
            VehicleSelectCmd = new AsyncRelayCommand<VehicleCategory>(SelectVehicle);
        }

        private async Task SelectVehicle(VehicleCategory? category)
        {
            if (category != null)
            {
                AppHelper.DriverVehicleType = category.VehicleType;
                await _navigationService.PopAsync();
            }
        }

        private void LoadVehicleTypes()
        {
            VehicleTypes =
        [
            new() { Id = 1, Name = "Sedan", VehicleType = VehicleTypeEnum.Sedan, Description = "Comfortable & efficient for city travel.", ImagePath = "vehicle_sedan.png" },
            new() { Id = 2, Name = "SUV", VehicleType = VehicleTypeEnum.SUV, Description = "Spacious for families & extra luggage.", ImagePath = "vehicle_suv.png" },
            new() { Id = 3, Name = "Bike", VehicleType =  VehicleTypeEnum.BikeTaxi, Description = "Quick and nimble for crowded streets.", ImagePath = "vehicle_bike.png" },
            new() { Id = 4, Name = "Van", VehicleType = VehicleTypeEnum.AutoRickshaw, Description = "Ideal for larger groups or cargo needs.", ImagePath = "vehicle_van.png" }
        ];
        }

    }

    public class VehicleCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public VehicleTypeEnum VehicleType { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
    }
}
