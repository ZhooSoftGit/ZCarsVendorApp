using System.Text.Json;
using ZCars.Model.Common;

namespace ZCarsDriver.Core.Stoage
{
    public static class RideStorageService
    {
        #region Constants

        private const string RideKey = "ongoing_ride_info";

        #endregion

        #region Methods

        public static void Clear()
        {
            Preferences.Remove(RideKey);
        }

        public static CurrentRide? Load()
        {
            if (!Preferences.ContainsKey(RideKey))
                return null;

            var json = Preferences.Get(RideKey, string.Empty);
            return string.IsNullOrEmpty(json) ? null : JsonSerializer.Deserialize<CurrentRide>(json);
        }

        public static void Save(CurrentRide ride)
        {
            var json = JsonSerializer.Serialize(ride);
            Preferences.Set(RideKey, json);
        }

        #endregion
    }

}
