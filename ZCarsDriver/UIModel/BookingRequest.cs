using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCars.Model.DTOs.DriverApp;

namespace ZCarsDriver.UIModel
{
    public class DriverLocation
    {
        public string DriverId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class BookingRequest : BookingRequestModel
    {
        
    }


}
