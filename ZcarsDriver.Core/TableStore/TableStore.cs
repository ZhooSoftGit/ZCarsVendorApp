using ZCarsDriver.Core.DBModel;
using ZhooSoft.LocalData.DataStore;
using ZhooSoft.LocalData.Util;

namespace ZCarsDriver.Core.TableStore
{
    public class LiveRideDetailsHandler : BaseStore<LiveRideDetails>, ILiveRideDetailsStore
    {
        #region Constructors

        public LiveRideDetailsHandler(IDBUtil dBUtil) : base(dBUtil)
        {
        }

        #endregion
    }
}
