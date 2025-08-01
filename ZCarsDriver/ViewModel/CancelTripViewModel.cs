using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using ZCarsDriver.Helpers;
using ZCarsDriver.Services;
using ZCarsDriver.Services.Contracts;
using ZhooSoft.Core;

namespace ZCarsDriver.ViewModel
{
    public partial class CancelReason : ObservableObject
    {
        #region Fields

        [ObservableProperty]
        private bool _isSelected;

        #endregion

        #region Properties

        public string Reason { get; set; }

        #endregion
    }

    public partial class CancelTripViewModel : ViewModelBase
    {
        #region Fields

        [ObservableProperty]
        private ObservableCollection<CancelReason> _cancelReasons;

        [ObservableProperty]
        private string _reasonTxt;

        [ObservableProperty]
        private CancelReason _selectedReason;

        #endregion

        #region Constructors

        public CancelTripViewModel()
        {
            Initiate();
            SubmitCommand = new AsyncRelayCommand(Submit);
            CloseCommand = new AsyncRelayCommand(Close);
            SelectReasonCommand = new RelayCommand<CancelReason>((obj) => OnSelect(obj));
            PageTitleName = "Cancel Trip";
        }

        #endregion

        #region Properties

        public IAsyncRelayCommand CloseCommand { get; }

        public IRelayCommand SelectReasonCommand { get; }

        public IAsyncRelayCommand SubmitCommand { get; }

        #endregion

        #region Methods

        private async Task Close()
        {
            Console.WriteLine("Popup closed");
            await _navigationService.PopAsync();
        }

        private void Initiate()
        {
            var cancelreasons = new List<CancelReason>
            {
                new CancelReason{ Reason = "No response", IsSelected = false},
                new CancelReason{ Reason = "Request to cancel", IsSelected = false},
                new CancelReason{ Reason = "Time change", IsSelected = false},
                new CancelReason{ Reason = "Others", IsSelected = false},
            };
            CancelReasons = new ObservableCollection<CancelReason>(cancelreasons);
        }

        private void OnSelect(CancelReason reason)
        {
            CancelReasons.Select(x => x.IsSelected = false);
            reason.IsSelected = true;
            SelectedReason = reason;
        }

        private async Task Submit()
        {
            try
            {
                if (SelectedReason == null)
                {
                    Console.WriteLine("Please select a reason before submitting.");
                    return;
                }
                Console.WriteLine($"Selected reason: {SelectedReason}");
                await ServiceHelper.GetService<IRideTripService>().CancelTripAsync(new ZhooCars.Model.DTOs.CancelTripDto
                {
                    RideTripId = 1
                });
                await ServiceHelper.GetService<DriverSignalRService>().NotifyCancelTrip();
                //call the API
                AppHelper.CurrentRide = null;
                await _navigationService.PopAsync();
            }
            catch (Exception ex)
            {
                await _navigationService.PopAsync();
            }
        }

        #endregion
    }
}
