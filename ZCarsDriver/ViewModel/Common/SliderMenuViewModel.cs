using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZCarsDriver.Helpers;
using ZhooSoft.Core;

namespace ZCarsDriver.ViewModel
{
    public partial class SliderMenuViewModel : ViewModelBase
    {
        public string Title { get; set; } = "Menu";
        public string Message { get; set; } = "Choose an option";
        public event Action OnClosed;
        public ICommand CloseCommand { get; }

        public ICommand MenuCommand { get; }

        public ICommand LogoutCmd { get; }

        public SliderMenuViewModel()
        {
            // Initialize the command
            MenuCommand = new Command<string>(async (obj) => await OnMenuSelected(obj));

            LogoutCmd = new RelayCommand(OnLogout);
            // Optional: trigger a method on constructor
            Initialize();
        }

        private void OnLogout()
        {
            AppHelper.OnLogout();
        }

        private async Task OnRideHistory()
        {
            //await _navigationService.PushAsync(ServiceHelper.GetService<RideListPage>());
        }

        private void Initialize()
        {
            // This method runs when VM is constructed
            // e.g. load data, log analytics, etc.
        }

        private async Task OnMenuSelected(string action)
        {
            switch (action)
            {
                case "RideHistory":
                    await OnRideHistory();
                    break;

                case "Settings":
                    //await _navigationService.PushAsync(ServiceHelper.GetService<SettingsPage>());
                    break;

                case "Help":
                    await Launcher.OpenAsync(new Uri("https://www.zhoosoft.com"));
                    break;

                    // add other menu items as needed
            }
        }
    }
}
