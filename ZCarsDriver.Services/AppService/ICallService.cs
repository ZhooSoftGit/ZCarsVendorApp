namespace ZCarsDriver.Services.AppService
{
    #region Interfaces

    public interface ICallService
    {
        #region Methods

        Task MakePhoneCall(string phoneNumber);

        #endregion
    }

    #endregion
}
