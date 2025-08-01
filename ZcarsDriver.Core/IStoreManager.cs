using SQLite;

namespace ZCarsDriver.Core
{
    #region Interfaces

    public interface IStoreManager
    {
        #region Methods

        void ClearAppDB();

        void ClearDB();

        void DropTable(SQLiteConnection db);

        Task<bool> Init(string key);

        #endregion
    }

    #endregion
}
