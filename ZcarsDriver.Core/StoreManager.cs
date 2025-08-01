using SQLite;
using ZCarsDriver.Core.DBModel;
using ZhooSoft.LocalData;
using ZhooSoft.LocalData.Util;

namespace ZCarsDriver.Core
{
    public class StoreManager : IStoreManager
    {
        #region Fields

        private readonly IDBUtil _dbUtil;

        #endregion

        #region Constructors

        public StoreManager(IDBUtil dBUtil)
        {
            _dbUtil = dBUtil;
        }

        #endregion

        #region Methods

        public void ClearAppDB()
        {
            throw new NotImplementedException();
        }

        public void ClearDB()
        {
            throw new NotImplementedException();
        }

        public void DropTable(SQLiteConnection db)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Init(string key)
        {
            try
            {
                string dBVerStr = await SecureStorage.GetAsync(DBConstants.DB_VERSION_LABEL);

                if (string.IsNullOrEmpty(dBVerStr) || !dBVerStr.Equals(DBConstants.DB_VERSION.ToString()))
                {
                    await SecureStorage.SetAsync(DBConstants.DB_VERSION_LABEL, DBConstants.DB_VERSION);
                    await _dbUtil.CreateDataBaseAsync(DBConstants.DatabasePath, true, key);
                }
                else
                {
                    await _dbUtil.CreateDataBaseAsync(DBConstants.DatabasePath, true, "zhoosoft");
                }

                var db = _dbUtil.GetDataBase();

                CreateTables(db);

            }
            catch (Exception ex)
            {
                var error = ex.Message;
                return false;
            }
            return true;
        }

        private void CreateTables(SQLiteConnection db)
        {
            db.CreateTable<VehicleModel>();
            db.CreateTable<UserModel>();
        }

        #endregion
    }
}
