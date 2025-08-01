using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ZCarsDriver.Services.Contracts;
using ZCarsDriver.UIModel;
using ZhooSoft.Core;

namespace ZCarsDriver.ViewModel
{
    public partial class PeakHoursViewModel : ViewModelBase
    {
        #region Fields

        [ObservableProperty]
        private ObservableCollection<string> _locations;

        [ObservableProperty]
        private DateTime _selectedDate;

        [ObservableProperty]
        private string _selectedLocation;

        #endregion

        #region Constructors

        public PeakHoursViewModel()
        {
            PageTitleName = "Peak hours";
            RefreshCmd = new Command(async () => await OnRefresh());
            _peakHoursService = ServiceHelper.GetService<IPeakHoursService>();
        }

        #endregion

        #region Properties

        public ICommand RefreshCmd { get; }

        public ObservableCollection<TimeSlot> TimeSlots { get; } = new();

        private IPeakHoursService _peakHoursService { get; }

        #endregion

        #region Methods

        public async override void OnAppearing()
        {
            IsBusy = true;
            base.OnAppearing();
            await LoadLocations();
            await LoadTimeSlots();
            IsBusy = false;
        }

        private async Task LoadLocations()
        {
            Locations = new ObservableCollection<string> { "Erode", "Salem", "CBE" };
        }

        private async Task LoadTimeSlots()
        {
            var result = await _peakHoursService.GetAllPeakHoursAsync();

            TimeSlots.Clear();

            // Example static data – replace with API or database if needed
            TimeSlots.Add(new TimeSlot { FromTime = new TimeSpan(8, 30, 0), ToTime = new TimeSpan(12, 30, 0) });
            TimeSlots.Add(new TimeSlot { FromTime = new TimeSpan(17, 0, 0), ToTime = new TimeSpan(22, 30, 0) });
        }

        private async Task OnRefresh()
        {
            IsBusy = true;
            await LoadTimeSlots();
            IsBusy = false;
        }

        #endregion
    }
}
