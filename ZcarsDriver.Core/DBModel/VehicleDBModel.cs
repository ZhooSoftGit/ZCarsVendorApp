using SQLite;
using ZhooCars.Model.DTOs;
using ZhooSoft.LocalData.DataStore;

namespace ZCarsDriver.Core.DBModel
{
    public class UserModel : UserDetailDto, IBaseDataObject
    {
        #region Properties

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        #endregion
    }

    public class VehicleModel : VehicleModelDto, IBaseDataObject
    {
        #region Properties

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        #endregion
    }
}
