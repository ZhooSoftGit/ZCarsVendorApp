using System.Text.Json;

namespace ZCarsDriver.Core
{
    public class GenericPreferenceService
    {
        #region Fields

        private static readonly Lazy<GenericPreferenceService> lazy =
            new(() => new GenericPreferenceService());

        #endregion

        #region Constructors

        private GenericPreferenceService()
        {
        }

        #endregion

        #region Properties

        public static GenericPreferenceService Instance => lazy.Value;

        #endregion

        #region Methods

        public void ClearAll()
        {
            Preferences.Clear();
        }

        public T Get<T>(string key) where T : class
        {
            string json = Preferences.Get(key, null);
            return json == null ? null : JsonSerializer.Deserialize<T>(json);
        }

        public void Set<T>(string key, T data)
        {
            string json = JsonSerializer.Serialize(data);
            Preferences.Set(key, json);
        }

        #endregion
    }
}
