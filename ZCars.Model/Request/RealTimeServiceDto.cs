namespace ZhooCars.Model.Request
{
    public class DistanceRequest
    {
        #region Properties

        public double DestinationLatitude { get; set; }

        public double DestinationLongitude { get; set; }

        public double OriginLatitude { get; set; }

        public double OriginLongitude { get; set; }

        #endregion
    }

    public class DistanceResponse
    {
        #region Properties

        public double DistanceInMeters { get; set; }

        public string DistanceText { get; set; }

        public double DurationInSeconds { get; set; }

        public string DurationText { get; set; }

        #endregion
    }
}
