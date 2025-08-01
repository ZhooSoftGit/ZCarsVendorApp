using ZCars.Model.DTOs.DriverApp;
using ZhooCars.Common;
using ZhooCars.Model.DTOs;

namespace ZCars.Model.Common
{
    public class CurrentRide
    {
        #region Fields

        private RideStatus _currentStatus = RideStatus.Assigned;

        #endregion

        #region Properties

        public BookingRequestModel BookingRequest { get; set; }

        public RideStatus CurrentStatus
        {
            get
            {
                return _currentStatus;
            }
            set
            {
                _currentStatus = value;
            }
        }

        public RideTripDto RideDetails { get; set; }

        #endregion

        #region Methods

        public RideStatus NextStatus()
        {
            if (CurrentStatus == RideStatus.Assigned)
            {
                return RideStatus.Reached;
            }
            else if (CurrentStatus == RideStatus.Reached)
            {
                return RideStatus.Started;
            }
            else if (CurrentStatus == RideStatus.Started)
            {
                return RideStatus.Completed;
            }
            else
            {
                return CurrentStatus;
            }
        }

        #endregion
    }
}
