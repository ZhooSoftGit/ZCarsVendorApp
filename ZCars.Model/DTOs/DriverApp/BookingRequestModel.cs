using ZhooCars.Common;

namespace ZCars.Model.DTOs.DriverApp
{
    public class BookingRequestModel
    {
        #region Properties

        public RideTypeEnum BookingType { get; set; }

        public int BoookingRequestId { get; set; }

        public string DistanceAndPayment { get; set; }

        public string DriverId { get; set; }

        public double DropLatitude { get; set; }

        public double DropLongitude { get; set; }

        public string DropoffLocation { get; set; }

        public string Fare { get; set; }

        public string PickupAddress { get; set; }

        public double PickupLatitude { get; set; }

        public string PickupLocation { get; set; }

        public double PickupLongitude { get; set; }

        public string PickupTime { get; set; }

        public int RemainingBids { get; set; }

        public string UserId { get; set; }

        public string? UserName { get; set; }

        #endregion
    }
}
