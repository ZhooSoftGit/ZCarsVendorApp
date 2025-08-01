using System.Text.Json;
using ZCarsDriver.Services.AppService;

namespace ZCarsDriver.Services
{
    public class OsrmService : IOsrmService
    {
        #region Fields

        private readonly HttpClient _httpClient;

        #endregion

        #region Constructors

        public OsrmService()
        {
            _httpClient = new HttpClient();
        }

        #endregion

        #region Methods

        public async Task<(double? Distance, List<Location> Locations, double? duration)> GetRoutePoints(double startLat, double startLng, double endLat, double endLng)
        {
            try
            {
                string accessToken = "pk.eyJ1Ijoiemhvb3NvZnQiLCJhIjoiY21iYWdmdDFyMTYyaDJrczl0OW04OTloMyJ9.NgqcQ7l55QagG0OpknCL-A";

                string url = $"https://api.mapbox.com/directions/v5/mapbox/driving/" +
                $"{startLng},{startLat};{endLng},{endLat}" +
                             $"?geometries=geojson&overview=full&access_token={accessToken}";

                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    return (null, null, null);

                var content = await response.Content.ReadAsStringAsync();
                var json = JsonDocument.Parse(content);

                var route = json.RootElement.GetProperty("routes")[0];

                double distance = route.GetProperty("distance").GetDouble();
                double duration = route.GetProperty("duration").GetDouble();

                var coordinates = route.GetProperty("geometry").GetProperty("coordinates");
                var locations = new List<Location>();

                foreach (var point in coordinates.EnumerateArray())
                {
                    double lng = point[0].GetDouble();
                    double lat = point[1].GetDouble();
                    locations.Add(new Location { Latitude = lat, Longitude = lng });
                }

                return (distance, locations, duration);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching OSRM route: {ex.Message}");
                return (null, null, null);
            }
        }

        private void osrm(double startLat, double startLng, double endLat, double endLng)
        {
        }

        #endregion
    }
}
