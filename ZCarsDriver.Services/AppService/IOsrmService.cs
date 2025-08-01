using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCarsDriver.Services.AppService
{
    #region Interfaces

    public interface IOsrmService
    {
        #region Methods

        Task<(double? Distance, List<Location> Locations, double? duration)> GetRoutePoints(double startLat, double startLng, double endLat, double endLng);

        #endregion
    }

    #endregion
}
