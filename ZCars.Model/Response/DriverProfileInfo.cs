using ZhooCars.Model.DTOs;

namespace ZCars.Model.Response
{
    public class DriverProfileInfo
    {
        #region Properties

        public List<DocumentDto>? Documents { get; set; }

        public DriverDetailDto? DriverDetail { get; set; }

        public DriverVehicleLinkDto? DriverVehicleLink { get; set; }

        public UserDetailDto? UserDetail { get; set; }

        #endregion
    }
}
