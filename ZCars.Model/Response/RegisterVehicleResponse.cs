using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhooCars.Model.DTOs;

namespace ZCars.Model.Response
{
    public class RegisterVehicleResponse
    {
        public VehicleDto? Vehicles { get; set; }

        public List<DocumentDto>? Document { get; set; }
    }
}
