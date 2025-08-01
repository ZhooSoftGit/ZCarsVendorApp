namespace ZhooSoft.Core
{
    public static class GlobalConstants
    {
        #region Constants

        public const string APPUrl = $"{BaseUrl}api/";

        public const string BaseUrl = "https://zhoodrive-b8hwb4hxdsg7eeby.centralindia-01.azurewebsites.net/";

        public const string DevUrl = "";

        public const string LocalDevUrl = "http://192.168.1.3:7091/";

        #endregion

        #region Properties

        public static string UserDocs { get; set; } = "UserDocs";

        public static string UserDetailData { get; set; } = "Userdetail";

        public static string VehicleLinkData { get; set; } = "DriverLink";

        public static string VendorData { get; set; } = "Vendor";

        public static string DriverData { get; set; } = "Driver";

        #endregion
    }
}
