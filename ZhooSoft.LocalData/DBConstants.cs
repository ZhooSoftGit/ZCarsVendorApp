﻿namespace ZhooSoft.LocalData
{
    public static class DBConstants
    {
        #region Constants

        public const string DatabaseFilename = "ZCarsSQLite.db3";

        public const string DB_VERSION = "1.0";

        public const string DB_VERSION_LABEL = "dbversion";

        public const SQLite.SQLiteOpenFlags Flags =
        // open the database in read/write mode
        SQLite.SQLiteOpenFlags.ReadWrite |
        // create the database if it doesn't exist
        SQLite.SQLiteOpenFlags.Create |
        // enable multi-threaded database access
        SQLite.SQLiteOpenFlags.SharedCache;

        #endregion

        #region Properties

        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

        #endregion
    }
}
