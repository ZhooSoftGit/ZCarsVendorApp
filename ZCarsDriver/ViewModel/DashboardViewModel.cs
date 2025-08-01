﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using ZCarsDriver.Services.Session;
using ZCarsDriver.UIModel;
using ZCarsDriver.Views.Common;
using ZCarsDriver.Views.Driver;
using ZCarsDriver.Views.Vendor;
using ZhooCars.Common;
using ZhooSoft.Auth;
using ZhooSoft.Core;

namespace ZCarsDriver.ViewModel
{
    public partial class DashboardViewModel : ViewModelBase
    {
        #region Fields

        [ObservableProperty]
        private ObservableCollection<QuickAction> _quickActions;

        #endregion

        #region Constructors

        public DashboardViewModel()
        {
            PageTitleName = "Vendor Dashboard";
            GotoDashboard = new AsyncRelayCommand(OngotoDashboard);
            OnActionCmd = new AsyncRelayCommand<QuickAction>(async (obj) => await Onaction(obj));
            OpenSideBarCmd = new AsyncRelayCommand(OpenSideBar);
            LoadActions();
        }

        #endregion

        #region Properties

        public IAsyncRelayCommand GotoDashboard { get; }

        public IAsyncRelayCommand<QuickAction> OnActionCmd { get; }

        public IRelayCommand OpenSideBarCmd { get; }

        #endregion

        #region Methods

        public async override void OnAppearing()
        {
            base.OnAppearing();
            await ServiceHelper.GetService<IUserSessionManager>().SetUserPreference("CurrentModule", UserRoles.Owner.ToString());
        }

        private void LoadActions()
        {
            QuickActions = new ObservableCollection<QuickAction>
                {
                    new QuickAction { Name = "Vechicles", Icon = "car_icon.png", Action = Helpers.ActionEnum.Vehicles },
                    new QuickAction { Name = "Your Rides", Icon = "car_icon.png", Action = Helpers.ActionEnum.Rides },
                    new QuickAction { Name = "Earnings", Icon = "car_icon.png", Action = Helpers.ActionEnum.Earnings },
                    new QuickAction { Name = "PeekHours", Icon = "car_icon.png", Action = Helpers.ActionEnum.PeekHrs },
                    new QuickAction { Name = "Reports", Icon = "car_icon.png", Action = Helpers.ActionEnum.Reports },
                    new QuickAction { Name = "Suggestions", Icon = "car_icon.png", Action = Helpers.ActionEnum.Others },
                    new QuickAction { Name = "others", Icon = "car_icon.png" },
                    new QuickAction { Name = "others", Icon = "car_icon.png" },
                };
        }

        private async Task Onaction(QuickAction obj)
        {
            if (obj is QuickAction action)
            {
                if (action.Action == Helpers.ActionEnum.Vehicles)
                {
                    await _navigationService.PushAsync(ServiceHelper.GetService<VehicleListPage>());
                }
                else if (action.Action == Helpers.ActionEnum.Rides)
                {
                    await _navigationService.PushAsync(ServiceHelper.GetService<RideListPage>());
                }
                else if (action.Action == Helpers.ActionEnum.Earnings)
                {
                    await _navigationService.PushAsync(ServiceHelper.GetService<EarningsPage>());
                }
                else if (action.Action == Helpers.ActionEnum.PeekHrs)
                {
                    await _navigationService.PushAsync(ServiceHelper.GetService<PeakHoursPage>());
                }
            }
        }

        private async Task OngotoDashboard()
        {
            var result = await _alertService.ShowConfirmation("Info", "Are you Leaving the vendor dashboard?", "Ok", "Cancel");

            if (result)
            {
                ServiceHelper.GetService<IMainAppNavigation>().NavigateToMain();
            }
        }

        private async Task OpenSideBar()
        {
            IsBusy = true;
            var result = await _navigationService.OpenPopup(new CommonMenu());
            await Task.Delay(100);
            IsBusy = false;
        }

        #endregion
    }
}
