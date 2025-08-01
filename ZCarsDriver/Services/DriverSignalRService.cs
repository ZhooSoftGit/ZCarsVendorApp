using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.AspNetCore.SignalR.Client;
using ZCars.Model.DTOs.DriverApp;
using ZCarsDriver.DPopup;
using ZCarsDriver.Helpers;
using ZCarsDriver.NavigationExtension;
using ZCarsDriver.UIModel;
using ZhooCars.Common;
using ZhooSoft.Core;
using ZhooSoft.Core.Alerts;
using ZhooSoft.Core.NavigationBase;

namespace ZCarsDriver.Services
{
    public class DriverSignalRService
    {
        #region Fields

        private readonly string _hubUrl = "http://192.168.1.3:7091/hubs/location";// Update IP if needed

        private static string _driverId;

        private static bool IsInitialized = false;

        private HubConnection _connection;

        private System.Timers.Timer _locationTimer;

        #endregion

        #region Constructors

        public DriverSignalRService()
        {
        }

        #endregion

        #region Events

        public event Action<BookingRequestModel>? OnBookingReceived;

        #endregion

        #region Methods

        public async Task ConnectAsync()
        {
            try
            {
                if (_connection != null && _connection.State != HubConnectionState.Connected)
                {
                    await _connection.StartAsync();
                    Console.WriteLine("✅ Connected to SignalR hub.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Connection error: {ex.Message}");
            }
        }

        public async Task DisconnectAsync()
        {
            try
            {
                if (_connection != null)
                {
                    await _connection.StopAsync();
                    Console.WriteLine("🚪 Disconnected from SignalR hub.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Disconnect error: {ex.Message}");
            }
        }

        public async Task DummyTriggerforBookingRequest()
        {
            if (_connection?.State == HubConnectionState.Connected)
            {
                await _connection.InvokeAsync("dummytriggerforbookingrequest");
            }
        }

        public void Initialize(string driverId)
        {
            if (IsInitialized) return;

            _driverId = driverId;
            var url = $"{_hubUrl}?userId={_driverId}&role=driver";

            _connection = new HubConnectionBuilder()
                .WithUrl(url)
                .WithAutomaticReconnect()
                .Build();

            _connection.On<BookingRequestModel>("ReceiveBookingRequest", HandleBookingRequest);

            RegisterHandler();

            IsInitialized = true;
        }

        public async Task NotifyCancelTrip()
        {
            var boookingRequestId = AppHelper.CurrentRide?.BookingRequest.BoookingRequestId.ToString();
            try
            {
                await _connection.InvokeAsync("CancelTripNotification", boookingRequestId);
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<bool> NotifyCompleteTrip(string otp)
        {
            try
            {
                bool isStart = false;
                var ride = AppHelper.CurrentRide;
                var result = await _connection.InvokeAsync<bool>("VerifyTripOtp", ride.BookingRequest.BoookingRequestId.ToString(), otp, isStart);

                if (result)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task NotifyPickupReached()
        {
            try
            {
                var boookingRequestId = AppHelper.CurrentRide.BookingRequest.BoookingRequestId.ToString();
                await _connection.InvokeAsync("PickupReachedNotification", boookingRequestId);
            }
            catch (Exception ex)
            {

            }
        }

        public async Task NotifyStartPickup()
        {
            var boookingRequestId = AppHelper.CurrentRide.BookingRequest.BoookingRequestId.ToString();
            await _connection.InvokeAsync("StartPickupNotification", boookingRequestId);
        }

        public async Task<bool> NotifyStartTrip(string otp)
        {
            try
            {
                bool isStart = true;
                var ride = AppHelper.CurrentRide;
                var result = await _connection.InvokeAsync<bool>("VerifyTripOtp", ride.BookingRequest.BoookingRequestId.ToString(), otp, isStart);

                if (result)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task RespondToBookingUser(string userId, string bookingRequestId, RideStatus status)
        {
            try
            {
                if (_connection?.State == HubConnectionState.Connected)
                {
                    await _connection.InvokeAsync("RespondToBookingByDriver", userId, _driverId, bookingRequestId, status.ToString().ToLower());
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task SendLocationAsync(double lat, double lng)
        {
            if (_connection?.State == HubConnectionState.Connected)
            {
                var location = new DriverLocation
                {
                    DriverId = _driverId,
                    Latitude = lat,
                    Longitude = lng
                };

                await _connection.InvokeAsync("UpdateDriverLocation", location);
            }
        }

        public void StartSendingLocation(int intervalMs = 3000)
        {
            _locationTimer = new System.Timers.Timer(intervalMs);
            _locationTimer.Elapsed += async (_, _) =>
            {
                if (_connection?.State == HubConnectionState.Connected)
                {
                    var loc = await GetLocationAsync();
                    if (loc != null)
                    {
                        await SendLocationAsync(loc.Latitude, loc.Longitude);
                    }
                }
            };
            _locationTimer.Start();
        }

        public void StopSendingLocation() => _locationTimer?.Stop();

        private async Task<Location?> GetLocationAsync()
        {
            return await AppHelper.GetUserLocation();
        }

        private void HandleBookingRequest(BookingRequestModel booking)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                var popup = new BookingRequestPopup();
                var navigationService = ServiceHelper.GetService<INavigationService>();

                var result = await navigationService.OpenPopup(popup, new Dictionary<string, object>
            {
                { "RequestModel", booking }
            });

                if (result is RideStatus status)
                {
                    await RespondToBookingUser(booking.UserId, booking.BoookingRequestId.ToString(), status);

                    if (status == RideStatus.Assigned)
                    {
                        await ServiceHelper.GetService<IAppNavigation>().LaunchDriverDashBoard();
                    }
                }
            });
        }

        private void RegisterHandler()
        {
            _connection.On<string>("TripStarted", bookingRequestId =>
            {
                // Update UI for trip started
            });

            _connection.On<string>("TripCompleted", bookingRequestId =>
            {
                // Update UI for trip completed
            });

            _connection.On<string>("OnTripCancelled", bookingRequestId =>
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await ServiceHelper.GetService<IAlertService>().ShowAlert("OOPS", "Your trip has been cancelled", "ok");
                    await ServiceHelper.GetService<IAppNavigation>().LaunchDriverDashBoard();
                });
            });

            _connection.On<object>("OtpVerificationFailed", async data =>
            {
                try
                {
                    var json = System.Text.Json.JsonSerializer.Serialize(data);
                    var result = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(json);

                    if (result != null && result.TryGetValue("reason", out var reason))
                    {
                        await Toast.Make(reason, ToastDuration.Short).Show();
                    }
                }
                catch (Exception ex)
                {
                    await Toast.Make("Failed to parse OTP error", ToastDuration.Short).Show();
                }
            });
        }

        #endregion
    }

}
